# ADR 0006: REST API with ASP.NET Core Controllers for Flutter Integration

## Status

Accepted (provisional) — architectural decision only; `ManLearning.Api` does not exist yet.

## Context

ADR 0005 adopts Flutter as a new client platform. Flutter cannot call `ManLearning.Application`
services in-process the way `ManLearning.Web` does today (Blazor Server runs the same process as
the application services it injects). A network-facing API is required.

Current, verified facts about the codebase that constrain this decision:

- There is no existing API project, controller, or `Microsoft.AspNetCore.Mvc`/
  `Microsoft.AspNetCore.OpenApi`/Swashbuckle reference anywhere in `src/`.
- The Application layer already exposes a small, well-defined set of use cases as plain C#
  methods: `CourseCatalogService.GetCourseListAsync` / `GetCourseAsync`,
  `LessonProgressService.GetLessonStateAsync` / `StartLessonAsync`,
  `QuizAttemptService.SubmitQuizAttemptAsync`, and
  `LearnerDashboardService.GetDashboardAsync`. These map naturally onto a small number of
  resource-oriented HTTP endpoints (Courses, Lessons, Quizzes, Dashboard).
- `ManLearning.Domain` and `ManLearning.Infrastructure` must not be referenced directly by a
  client; `ManLearning.Application` already serves as the boundary between use cases and
  Infrastructure/Domain internals, consistent with the project's layered architecture
  (`.github/copilot-instructions.md` §4).
- The product's near-term client target is Flutter **Web** (ADR 0005), which constrains the
  transport choice: browser-native HTTP/JSON is directly supported, while gRPC requires an
  additional gRPC-Web proxy layer in front of a browser client.

## Decision

1. Introduce a new project, **`ManLearning.Api`**, as the HTTP entry point for Flutter (and any
   future non-Blazor client). This project is not created as part of this ADR; its creation is
   deferred to the implementation phase described in ADR 0005's migration strategy.
2. Use **REST over HTTP/JSON** as the API style, not gRPC.
3. Implement the API using **ASP.NET Core Controllers** (`[ApiController]`), not Minimal API.
4. `ManLearning.Api` depends only on `ManLearning.Application` (and transitively on
   `ManLearning.Domain`/`ManLearning.Infrastructure` through it, the same dependency shape
   `ManLearning.Web` already has). Controllers call Application services; they do not call
   Infrastructure or Domain types directly, and they do not expose Domain or Infrastructure types
   in request/response bodies.

Expected dependency shape (unchanged from the existing layered architecture, with
`ManLearning.Api` added alongside `ManLearning.Web`):

```text
Flutter
  ↓ HTTPS/JSON
ManLearning.Api
  ↓
Application
  ↓
Domain

Infrastructure
  ↓
Database
```

## Alternatives Considered

- **gRPC.** Rejected for now: Flutter Web (the near-term target per ADR 0005) cannot call gRPC
  services directly from a browser without a gRPC-Web proxy (e.g. Envoy), which would add new
  infrastructure the project does not otherwise need. gRPC's streaming strengths are not required
  by the current use cases (course browsing, quiz submission, dashboard reads).
- **ASP.NET Core Minimal API.** Rejected in favor of Controllers: the project already anticipates
  a moderate, growing number of resource groups (Courses, Lessons, Quizzes, Dashboard, and later
  Authentication and AI per ADR 0007 and the AI-readiness discussion in the research session).
  Controllers group related endpoints more readably at this scale and have more mature support for
  attribute-based authorization (`[Authorize]`) and OpenAPI metadata generation (needed by ADR
  0008), both of which Minimal API supports with comparatively more manual wiring.
- **Exposing `ManLearning.Application` (or Domain/Infrastructure types) directly to Flutter via a
  generic RPC/reflection mechanism.** Rejected: this would leak internal service shapes into the
  client contract and violate the project's layering rules
  (`.github/copilot-instructions.md` §4, §6); a dedicated API with its own DTOs is required.

## Consequences

- A new project (`ManLearning.Api`) will need to be added to `ManLearning.sln` when implementation
  begins; this ADR does not perform that step.
- `ManLearning.Domain`, `ManLearning.Application`, and `ManLearning.Infrastructure` require no
  structural changes as a direct result of this decision.
- `ManLearning.Web` (Blazor) and `ManLearning.Api` will temporarily coexist, both depending on the
  same `ManLearning.Application`, during the migration window described in ADR 0005.
- Authentication/authorization wiring for the API (who may call it, how the caller's identity is
  established) is addressed separately in ADR 0007, not here.
- If a future requirement emerges that REST/Controllers cannot express well (for example, a
  genuine need for bidirectional streaming), this ADR should be superseded by a new one rather
  than silently reworked in code.

## Migration / Implementation Notes

- No code, project file, or package reference is added by this ADR. `ManLearning.Api` creation,
  its initial endpoint set, and its addition to `ManLearning.sln` are implementation-phase work
  for a later change.
- A concrete initial endpoint sketch (not finalized) based on the current Application services:
  `GET /api/courses`, `GET /api/courses/{courseId}`, `GET /api/me/dashboard`,
  `GET /api/lessons/{lessonId}/progress`, `POST /api/lessons/{lessonId}/start`,
  `POST /api/quizzes/{quizId}/attempts`. These should be re-validated against the Application
  layer at implementation time, not assumed final from this ADR alone.
- Request/response DTOs for the API are a new concern distinct from the existing
  `ManLearning.Application.Learning.Dtos` types; whether the API reuses those DTOs directly or
  defines its own should be decided during implementation, guided by ADR 0008 (OpenAPI contract).
