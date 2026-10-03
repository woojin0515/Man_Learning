# ADR-0006 — REST API + ASP.NET Core Controllers

## Status

Accepted (provisional) — architectural decision only; `ManLearning.Api` does not exist yet and is
not created by this ADR.

## Context

ADR-0005 adopts Flutter (Web first) as a new client platform while keeping `ManLearning.Web`
(Blazor Server) running in production. This creates a concrete communication problem that this
ADR exists to solve: **Flutter cannot reach the current Application layer the way Blazor does.**

Verified, current facts about the repository that constrain this decision:

- `ManLearning.Web` has no HTTP API. `Program.cs` registers `CourseCatalogService`,
  `LessonProgressService`, `QuizAttemptService`, and `LearnerDashboardService` directly as scoped
  DI services (`builder.Services.AddScoped<...>()`), and Razor components (e.g.
  `Dashboard.razor`, `CourseList.razor`, `QuizForm.razor`) call them in-process via
  `@inject`. There is no network boundary between the UI and the Application layer today.
- There is no existing API project, controller, or `Microsoft.AspNetCore.Mvc` /
  `Microsoft.AspNetCore.OpenApi` / Swashbuckle package reference anywhere under `src/`.
- `ManLearning.Application` already exposes a small, well-defined set of use cases as plain C#
  methods — `CourseCatalogService.GetCourseListAsync` / `GetCourseAsync`,
  `LessonProgressService.GetLessonStateAsync` / `StartLessonAsync`,
  `QuizAttemptService.SubmitQuizAttemptAsync`, `LearnerDashboardService.GetDashboardAsync` — each
  already returning DTOs (`CourseDto`, `LessonDto`, `QuizAttemptResultDto`,
  `LearnerDashboardDto`, etc.) rather than Domain entities. These map naturally onto a small
  number of resource-oriented HTTP endpoints (Courses, Lessons, Quizzes, Dashboard).
- The current solution (`ManLearning.sln`) has five `src/` projects: `ManLearning.Web`,
  `ManLearning.Application`, `ManLearning.Domain`, `ManLearning.Infrastructure`,
  `ManLearning.Shared`. There is no `ManLearning.Api` project.
- `ManLearning.Domain` and `ManLearning.Infrastructure` have no dependency on ASP.NET Core or
  Blazor; `ManLearning.Application` is the existing boundary between use cases and
  Domain/Infrastructure internals (`.github/copilot-instructions.md` §4). Any new client-facing
  layer must respect that boundary rather than reach around it.
- The product's near-term client target is Flutter **Web** (ADR-0005), which constrains the
  transport choice: browser-native HTTP/JSON is directly supported today, while gRPC requires an
  additional gRPC-Web proxy in front of a browser client.
- Authentication is not implemented (ADR-0007 is a separate, not-yet-implemented decision), and
  API contract/documentation tooling (OpenAPI) is a separate, not-yet-implemented decision
  (ADR-0008). Both are referenced here only as boundaries this ADR must not duplicate or
  contradict.

## Decision

Adopt **REST + JSON, implemented with ASP.NET Core Controllers**, as the communication layer
between the Flutter client and the .NET backend.

A new project, **`ManLearning.Api`**, will be added to carry this layer:

```text
src/
├── ManLearning.Api/            (new — not created by this ADR)
├── ManLearning.Application/
├── ManLearning.Domain/
├── ManLearning.Infrastructure/
├── ManLearning.Shared/
└── ManLearning.Web/
```

`ManLearning.Api` is **not created, scaffolded, or added to `ManLearning.sln` as part of this
ADR.** Its creation is implementation-phase work for a later change, per ADR-0005's migration
strategy.

### Target architecture

```text
Flutter
   │
   │ HTTPS / JSON
   ▼
ManLearning.Api
   │
   │ Application service call
   ▼
ManLearning.Application
   │
   ▼
ManLearning.Domain
   │
   ▼
ManLearning.Infrastructure
   │
   ▼
Database
```

`ManLearning.Web` is retained unchanged throughout this decision. During the migration window
described in ADR-0005, both clients call the same `ManLearning.Application` through their own
entry points, in parallel:

```text
Existing Blazor
    │
    └── Application   (in-process DI call, unchanged)

Flutter
    │
    └── HTTPS
          │
          ▼
      ManLearning.Api
          │
          └── Application   (new HTTP-bound call)
```

### Responsibilities of `ManLearning.Api`

**`ManLearning.Api` is responsible for:**

- Exposing HTTP endpoints
- Handling HTTP request/response plumbing
- JSON serialization/deserialization
- Deciding HTTP status codes
- Being the HTTP boundary for authentication and authorization (the concrete mechanism is
  ADR-0007's responsibility, not this ADR's)
- API-boundary request validation (e.g. malformed/missing request fields) — distinct from Domain
  invariant validation, which already happens inside `ManLearning.Domain`
- Calling `ManLearning.Application` services
- Defining API-specific DTOs / contracts where the existing Application DTOs are not suitable for
  direct exposure
- Providing OpenAPI documentation of its endpoints (the generation/consumption policy itself is
  ADR-0008's responsibility, not this ADR's)

**`ManLearning.Api` is explicitly *not* responsible for:**

- Core business rules (lesson completion policy, quiz scoring, XP/level calculation, streak
  advancement — these remain exclusively in `ManLearning.Domain` / `ManLearning.Application`)
- Domain entity business behavior
- Direct database access
- Writing EF Core queries inside controllers
- Calling an AI provider or any other business/application-level external integration directly
- Flutter-specific business logic of any kind

Controllers must not look like this:

```csharp
// Rejected direction — controller bypasses Application and talks to the database directly.
[HttpGet]
public async Task<IActionResult> GetCourses()
{
    var courses = await dbContext.Courses.ToListAsync();
    return Ok(courses);
}
```

Controllers must instead preserve the existing layering:

```text
Controller
    ↓
Application service
    ↓
Domain / Infrastructure
```

This is the same boundary `ManLearning.Web`'s Razor components already respect by calling
Application services rather than `ManLearningDbContext` directly — `ManLearning.Api` adopts the
identical discipline, just across an HTTP boundary instead of an in-process DI boundary.

### DTO boundary

API responses are **not** Domain entities serialized as-is:

```text
Domain Entity
    ≠
API Response DTO
```

Where the existing `ManLearning.Application.Learning.Dtos` types (`CourseDto`, `LessonDto`,
`QuizAttemptResultDto`, `LearnerDashboardDto`, etc.) are already suitable as a wire format, the API
may reuse them directly; where they are not, `ManLearning.Api` defines its own API-specific DTOs.
**Neither the exact DTO naming nor any specific endpoint's request/response contract is decided
by this ADR** — that is implementation-phase work, to be carried out consistently with ADR-0008.

## Why REST

Evaluated against Man Learning's actual, current situation rather than general HTTP-API
folklore:

- The near-term client is **Flutter Web** (ADR-0005); REST/JSON is natively consumable from a
  browser-based Flutter client with no additional proxy infrastructure.
- JSON payloads are directly human-readable, which matters concretely here because the project has
  no existing API tooling or debugging setup (no Swashbuckle, no OpenAPI, no API test harness) —
  REST/JSON keeps the debugging story (browser dev tools, `curl`, Postman) trivial while that
  tooling is still being built.
- REST/JSON gives a conventional client/server boundary that maps directly onto the existing
  resource shape of the Application layer (Courses, Lessons, Quizzes, Dashboard) without requiring
  a new interface definition language.
- If Flutter Mobile is added later (ADR-0005 treats this as a future option, not a commitment),
  the same REST API serves it without any server-side change — mobile Flutter uses the same HTTP
  client stack as Flutter Web.
- REST connects naturally to OpenAPI (ADR-0008), which the project already intends to use as the
  API contract's single source of truth.
- If a requirement for another frontend or external client ever emerges, the same `ManLearning.Api`
  can serve it without rework, because REST/JSON is not tied to Flutter specifically.

## Why ASP.NET Core Controllers

Evaluated against the project's current and reasonably foreseeable scale:

- The endpoint surface is expected to grow from today's four Application services (Courses,
  Lessons, Learning Progress, Quiz) to additional resource groups already anticipated elsewhere in
  the project's own decisions and research: Authentication (ADR-0007) and, per the AI-readiness
  discussion in the Flutter migration research, an eventual AI Tutor surface. Controllers group
  these resources into separate, readable classes as they accumulate; a single Minimal API file
  (or even several `MapGet`/`MapPost` groups) becomes harder to navigate at this scale.
- Controllers give an explicit, discoverable HTTP boundary (one controller per resource,
  `[Route("api/courses")]`-style grouping) that keeps the API boundary visually distinct from
  `ManLearning.Application`'s service classes, reducing the risk of a controller silently growing
  business logic of its own.
- `[Authorize]`-attribute-based authorization and ASP.NET Core's OpenAPI metadata generation are
  more mature and more declarative on Controllers than on Minimal API endpoints, which matters
  because both authentication (ADR-0007) and OpenAPI documentation (ADR-0008) are near-term,
  already-decided-in-principle requirements for this API, not hypothetical future needs.
- Separating "API boundary" (`ManLearning.Api`, Controllers) from "use case" (`ManLearning.Application`,
  plain service classes) is easiest to enforce when the two layers also look structurally
  different (attribute-routed controllers vs. plain injected services) — Minimal API's
  function-per-endpoint style resembles Application service methods closely enough that the
  boundary would be easier to blur over time.

## Alternatives Considered

### 1. Minimal API

**Pros:** Less boilerplate per endpoint; faster to stand up a single endpoint; no controller base
class required.

**Cons at this project's scale:** As the number of resource groups grows (Courses → Lessons →
Quiz → Learning Progress → Authentication → AI Tutor), Minimal API endpoints tend to accumulate
either in one large file or across many small ad hoc extension-method groupings, with weaker
built-in conventions for organizing by resource than Controllers provide. Attribute-based
`[Authorize]` and OpenAPI grouping/documentation (both needed per ADR-0007/ADR-0008) require more
manual wiring than with Controllers.

**Decision for Man Learning:** Controllers are chosen. The project's growth trajectory (explicitly
already includes Authentication and an eventual AI surface) outweighs Minimal API's lower
per-endpoint boilerplate.

### 2. gRPC

**Pros:** Strong typed contracts via Protobuf; efficient binary serialization; built-in streaming
support.

**Cons for this product today:** The near-term client is Flutter **Web** (ADR-0005). Flutter Web
cannot call a gRPC service directly from the browser — it requires a gRPC-Web proxy (e.g. Envoy)
in front of the API, which is new infrastructure this project does not otherwise need. JSON/REST
is directly debuggable with a browser's network tab or `curl`; gRPC's binary wire format requires
additional tooling to inspect, which matters for a project that has no existing API debugging
setup. None of the current use cases (browsing courses, submitting a quiz attempt, reading a
dashboard) need gRPC's streaming capabilities.

**Decision for Man Learning:** REST/JSON is chosen. gRPC's benefits do not apply to a Flutter-Web-
first product and would add infrastructure (a gRPC-Web proxy) with no corresponding current need.

### 3. Keep Blazor calling `ManLearning.Application` directly (no new API layer)

**Pros:** Zero additional layer, zero additional DTOs, zero additional serialization overhead —
this is exactly today's working, in-production setup for `ManLearning.Web`.

**Cons:** This only works because Blazor Server and the Application services run in the same
process. Flutter is a separate client process (a browser tab or a mobile app) that cannot perform
an in-process DI call into `ManLearning.Application` — there is no mechanism by which Flutter could
"inject" `CourseCatalogService` the way a Razor component does. Continuing to rely solely on
in-process calls makes Flutter adoption (ADR-0005) impossible, not merely suboptimal.

**Decision for Man Learning:** This alternative is rejected as a path for Flutter specifically, but
is not being removed for Blazor — `ManLearning.Web` keeps calling `ManLearning.Application`
in-process exactly as it does today; only Flutter requires the new HTTP boundary.

## OpenAPI Relationship

This ADR establishes that the REST API is structured so it **can** be documented via OpenAPI (for
example, by using Controllers and typed DTOs rather than untyped/dynamic responses). **The
specific policy for generating, publishing, and consuming that OpenAPI document — including
whether/how a Dart client or models are generated from it — is ADR-0008's decision, not this
one.** This ADR does not duplicate or pre-empt that decision.

## Authentication Relationship

This ADR establishes only the principle that **`ManLearning.Api` is the HTTP boundary at which
authentication and authorization are enforced** — i.e., every request Flutter makes to
`ManLearning.Api` is a point where the caller's identity can and must be verified before reaching
`ManLearning.Application`. **The specific identity provider (Microsoft Entra External ID), token
validation mechanism, and how a validated identity is mapped to a `LearnerId` are ADR-0007's
decision, not this one.** This ADR does not duplicate or pre-empt that decision.

## Migration

`ManLearning.Web` is **not** removed as part of this ADR, and no migration step below is executed
by this ADR. The recommended path, consistent with ADR-0005, is:

```text
1. Keep the existing Blazor application running
2. Add ManLearning.Api
3. Implement one API vertical slice (e.g. Courses) end-to-end
4. Validate the OpenAPI contract produced by that slice (per ADR-0008)
5. Connect a Flutter client to that slice
6. Migrate remaining Flutter features incrementally, one Application use case at a time
7. Run Blazor and Flutter in parallel; validate feature and data parity between them
8. Only once Flutter has demonstrably replaced the relevant Blazor functionality, evaluate
   removing ManLearning.Web as a separate, explicit decision
```

No part of this migration path is executed by this ADR.

## Consequences

**Positive:**

- Clear separation between Flutter and the .NET backend, with `ManLearning.Api` as the only
  network-facing surface of the backend.
- Future mobile Flutter clients (if ADR-0005's future option is exercised) can reuse the same API
  with no backend change.
- `ManLearning.Api` can be tested independently of both Blazor and Flutter (e.g. via
  `WebApplicationFactory`-style integration tests), extending the project's existing test
  pyramid (`ManLearning.Domain.Tests`, `ManLearning.Application.Tests`,
  `ManLearning.Infrastructure.Tests`) with an API-level layer.
- The API is structured in a way that connects naturally to OpenAPI-based client generation
  (ADR-0008), without this ADR having to decide that generation policy itself.
- Establishes a clear deployment/change boundary between frontend(s) and backend: `ManLearning.Api`
  can evolve its contract deliberately rather than frontend and backend being forced to change in
  lockstep as they effectively are today inside `ManLearning.Web`.

**Costs / trade-offs:**

- A new layer (`ManLearning.Api`) and new DTOs are introduced where none existed before.
- HTTP serialization/deserialization overhead is introduced where today's Blazor-to-Application
  call is a plain in-process method call.
- Authentication and API-level error handling must now be designed explicitly for this new
  boundary (left to ADR-0007 and future implementation work; not solved by this ADR).
- Overall initial implementation effort is higher than continuing to rely solely on
  `ManLearning.Web`'s existing direct-call structure — this cost is accepted because it is the
  precondition for Flutter adoption (ADR-0005), not avoidable within that decision.

## Important — Not Performed By This ADR

The following actions are explicitly **not** performed as part of this ADR:

- Creating the `ManLearning.Api` project
- Creating any controller
- Implementing any endpoint
- Adding any NuGet package
- Writing any Flutter code
- Modifying any Blazor code
- Changing the database schema
- Changing any Azure resource
- Implementing authentication
- Installing any OpenAPI generator

This ADR's success condition is that **ADR-0006 is written consistently with the existing ADR
format, and that the REST API boundary between Flutter and .NET, together with
`ManLearning.Api`'s responsibilities, is clearly decided** — not that any of the above
implementation work has begun.
