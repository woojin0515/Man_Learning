# ADR 0005: Flutter Adoption as the New Client Platform

## Status

Accepted (provisional) — adoption decision only; no Flutter code exists yet.

## Context

`ManLearning.Web` is a Blazor Web App (Blazor Server, `InteractiveServer` render mode) built on
.NET 10. The repository's investigation of a Flutter/Dart-based client (session research,
2026-10-03) confirmed the following facts about the current codebase, which this ADR treats as
ground truth:

- `ManLearning.Domain`, `ManLearning.Application`, and `ManLearning.Infrastructure` have no
  dependency on ASP.NET Core, Blazor, or any UI framework. Application services
  (`CourseCatalogService`, `LessonProgressService`, `QuizAttemptService`,
  `LearnerDashboardService`) are already UI-agnostic and are consumed by `ManLearning.Web` only
  through dependency injection.
- `ManLearning.Web` currently has **no REST/HTTP API** — Razor components call Application
  services directly in-process. A non-Blazor client cannot reach these services without a new
  API layer (see ADR 0006).
- 49 tests pass across `ManLearning.Domain.Tests`, `ManLearning.Application.Tests`, and
  `ManLearning.Infrastructure.Tests`; none of them depend on Blazor.
- The product is deployed to a single Azure App Service (`manlearning-woojin-krc`) with Azure SQL
  Database (Serverless) as the production store; this is already operating and must not be
  disrupted mid-migration.
- No authentication exists today. `ManLearning.Web.Learners.CurrentLearnerContext` issues a new
  `LearnerId.New()` per Blazor Server circuit and is explicitly documented in code as "not a
  security boundary."

Separately, there is product interest in a Flutter-based client, motivated by both team skill
development and the possibility of a future mobile learner experience. This ADR decides whether,
and how, to adopt Flutter without assuming the current Blazor application must be replaced
immediately or that a mobile app is required now.

## Decision

Adopt **Flutter** as a new client platform, with the following scope:

1. **Flutter Web first.** The initial Flutter client targets the web, matching the current
   product's delivery mechanism (a browser-based learning app with no stated native-mobile
   requirement today).
2. **Flutter Mobile (Android/iOS) is a future option, not committed now.** Flutter's
   cross-platform design means adding mobile targets later does not require restructuring the
   Flutter codebase decided here; it is deferred until there is a concrete mobile requirement.
3. **Flutter is adopted as the UI/client layer only.** `ManLearning.Domain`,
   `ManLearning.Application`, and `ManLearning.Infrastructure` are retained as-is and remain the
   single source of truth for business rules (lesson completion, quiz scoring, XP, levels,
   streaks). No business logic is duplicated into Dart.
4. **The existing Blazor application (`ManLearning.Web`) is not deleted now.** It continues to
   run in production throughout the migration. It will only be retired after the conditions in
   ADR 0005's migration strategy (below) are met.
5. **Migration strategy — sequential, not a rewrite:**
   ```text
   Keep Blazor running
          ↓
   Build the API layer (ADR 0006)
          ↓
   Build the Flutter client against that API
          ↓
   Run Blazor and Flutter in parallel; validate feature and data parity
          ↓
   Retire Blazor only once Flutter is verified equivalent
   ```
   This is chosen over an immediate cutover because the application is live in Azure today with
   real users, and because no API layer exists yet for Flutter to call.

## Alternatives Considered

- **Delete Blazor immediately and build Flutter in its place.** Rejected: there is no API layer
  for Flutter to use yet, so this would require a big-bang rewrite with no safety net and no way
  to validate behavior against the current system before cutover.
- **Run Blazor and Flutter side by side indefinitely.** Rejected as a long-term state (though it
  is an intentional, temporary phase of the chosen strategy): maintaining two frontends
  permanently duplicates UI maintenance cost with no corresponding benefit once Flutter is
  proven.
- **Flutter Mobile first (or Flutter Web + Mobile simultaneously).** Rejected for the initial
  adoption: the product has no stated mobile-specific requirement today, and committing to mobile
  now would be scope expansion not justified by current product needs (see
  `.github/copilot-instructions.md` §2, "avoid adding features simply because they are
  technically interesting").
- **Keep Blazor only and do not adopt Flutter.** Rejected: out of scope for this ADR, which
  records the adoption decision as requested; the comparative UI-framework tradeoffs were
  evaluated in the preceding research session.

## Consequences

- `ManLearning.Domain`, `ManLearning.Application`, and `ManLearning.Infrastructure` require no
  changes as a direct result of this decision; their existing 49 tests remain the validation
  baseline for business logic regardless of which client consumes them.
- An API layer becomes mandatory before any Flutter work can begin (see ADR 0006); Flutter cannot
  be built against the current in-process DI structure.
- Two clients (Blazor and Flutter) will exist simultaneously for a transitional period. This is an
  accepted, deliberate cost of the chosen migration strategy, not an oversight.
- `ManLearning.Web` removal is **out of scope for this ADR** and must not happen until the
  parallel-validation phase of the migration strategy above is complete.
- If product requirements later demand Flutter Mobile, or demand abandoning Flutter Web in favor
  of mobile-first, this ADR should be superseded by a new one rather than silently reinterpreted.

## Migration / Implementation Notes

- No Flutter project, package, or code is created by this ADR. Project scaffolding begins only in
  a later "Flutter foundation" phase, after ADR 0006–0009 are in place.
- The phased removal of `ManLearning.Web` should itself be tracked as an explicit, separate future
  change (not silently done as a side effect of adding Flutter), so that production stability is
  never put at risk without a documented rollback point.
