# ADR 0008: OpenAPI as the API Contract Between `ManLearning.Api` and Flutter

## Status

Accepted (provisional) — contract strategy only; no OpenAPI generation tooling is installed and
no Dart client/model generator is selected yet.

## Context

ADR 0006 establishes `ManLearning.Api` (REST/JSON, ASP.NET Core Controllers) as the integration
point for Flutter. C# and Dart cannot share types directly, so the request/response shapes that
`ManLearning.Api` exposes need an explicit, maintained contract that both sides agree on.

`ManLearning.Shared` currently exists as an essentially empty project (a single placeholder
`Class1.cs`) and is not referenced by any other project in the solution. It is not, today, a
contract store of any kind, and this ADR does not change that.

## Decision

Use **OpenAPI as the single source of truth for the API contract** between `ManLearning.Api` and
the Flutter client:

1. `ManLearning.Api` generates an OpenAPI specification describing its endpoints and DTOs (using
   ASP.NET Core's built-in OpenAPI document generation once that project exists).
2. The API contract is **not** hand-copied into separate C# and Dart model definitions maintained
   independently. The OpenAPI document is the authoritative description of request/response
   shapes; both the server's own DTOs and the Flutter client's models are expected to stay
   consistent with it.
3. Where practical, Dart models and an API client for Flutter are **generated from the OpenAPI
   document** rather than hand-written, to avoid manual drift between the server contract and the
   client.
4. **No specific OpenAPI-to-Dart generator is selected by this ADR.** Before committing to one,
   implementation work must run a small, isolated vertical spike that generates Dart code from a
   real (even if minimal) `ManLearning.Api` OpenAPI document, and checks:
   - the quality/idiomaticity of the generated Dart code,
   - compatibility with the Flutter stack chosen in ADR 0009 (Riverpod) and the HTTP/serialization
     choices expected alongside it (e.g. Dio-based clients, freezed-style immutable models),
   - how well the generator handles contract evolution (adding/removing fields, versioning).
   The outcome of that spike should be recorded as a follow-up ADR or an update to this one, not
   assumed here.

## Alternatives Considered

- **Hand-maintain separate C# and Dart DTOs with no shared source of truth.** Rejected: this
  requires manually keeping two independent type definitions in sync on every API change, which
  does not scale as the API grows (Courses, Lessons, Quizzes, Dashboard, and later Authentication
  and AI endpoints) and has no automated way to detect drift or breaking changes.
- **Introduce a separate schema/contract project or IDL (e.g. a hand-maintained JSON Schema or
  Protobuf-like schema project) as the source of truth instead of OpenAPI.** Rejected for the
  current project scale: this would mean maintaining a second schema artifact in addition to the
  ASP.NET Core API itself, with a smaller Dart tooling ecosystem than OpenAPI-based generation
  already provides, and no corresponding benefit for a REST/JSON API (per ADR 0006).
- **Using `ManLearning.Shared` as a hand-written contract store shared by convention (not by
  compiled code, since C#/Dart cannot share assemblies).** Rejected: it would still require manual
  duplication into Dart with no automated verification, which is the same drift problem as the
  first alternative; it does not solve anything OpenAPI generation does not already solve better.

## Consequences

- `ManLearning.Api` must be designed so its controllers and DTOs produce a clean, generator-
  friendly OpenAPI document (clear, named request/response types; consistent naming) — this is a
  constraint on future API implementation, not something satisfied by this ADR alone.
- Until the generator spike (above) is run, no Dart model/client generation pipeline exists; this
  is intentionally left open rather than prescribed.
- `ManLearning.Shared` remains unused by this decision. Whether it ever becomes useful (for
  genuinely shared non-generated constants, for example) is unrelated to this ADR and must not be
  assumed from it.
- If a future requirement reveals OpenAPI-based generation is unworkable for this project (for
  example, if the spike in the Migration/Implementation Notes below fails to produce usable Dart
  code), this ADR should be superseded by a new one rather than silently abandoned in code.

## Migration / Implementation Notes

- No OpenAPI package, generator, or Dart dependency is installed by this ADR.
- Recommended next step at implementation time: once `ManLearning.Api` exists with at least one
  endpoint (per ADR 0006's migration notes), run a small spike that (a) generates its OpenAPI
  document and (b) feeds it through a candidate Dart generator to evaluate output quality before
  committing to a specific tool.
- The generator choice and its evaluation results should be documented as a follow-up decision
  (either amending this ADR once accepted, or as a new ADR) rather than decided silently during
  feature implementation.
