# ADR 0009: Riverpod for Flutter State Management

## Status

Accepted (provisional) — state management choice only; no Flutter project exists yet.

## Context

ADR 0005 adopts Flutter (Web first) as a new client platform. ADR 0006 establishes that the
Flutter client will talk to `ManLearning.Api` over REST/JSON rather than holding business logic
locally; per the project's guiding principle that the server is the single source of truth for
business rules (Domain/Application), the Flutter client's own internal complexity is primarily
about **managing UI state, API call state (loading/success/error), and authentication state** —
not business rules.

The current Blazor UI (`ManLearning.Web.Components.Pages.*`) keeps this kind of state as plain
private fields in each component's `@code` block (for example, `Dashboard.razor`'s `_dashboard`
and `_loadError` fields, reset and reassigned around each async load). A Flutter equivalent needs
a comparable, but idiomatically Flutter, way to represent "not yet loaded / loaded / failed" for
data coming from `ManLearning.Api`, plus a place to hold cross-cutting state such as "is the user
authenticated" (relevant once ADR 0007 is implemented) without resorting to ad hoc global
mutable singletons.

## Decision

Adopt **Riverpod** as the state management approach for the Flutter client, with the following
intent:

1. Riverpod is used to represent **API/business state** (data fetched from `ManLearning.Api`,
   wrapped in `AsyncValue`-style loading/success/error semantics) separately from **UI-local
   state** (for example, form input values, which selection is currently highlighted) that belongs
   in widget state, not in a Riverpod provider.
2. The Flutter architecture pairs Riverpod with a **feature-oriented structure**: state, API
   access, and UI for a given feature (e.g. Courses, Lessons/Quiz, Dashboard, Authentication) are
   organized together, rather than grouped purely by technical layer across the whole app. This
   ADR records the feature-oriented *principle*; the exact directory layout is deferred (see
   Migration/Implementation Notes).
3. Riverpod's provider scoping is also used to represent authentication state (once ADR 0007 is
   implemented) as an explicit, observable piece of state the rest of the app reacts to, instead
   of as an ad hoc global mutable singleton.
4. Riverpod's own dependency-injection-like provider graph is relied upon for wiring repositories
   and API clients; **no separate DI framework (e.g. `get_it`) is introduced**, to avoid two
   overlapping mechanisms for the same concern.
5. This ADR does not decide the final Flutter project directory structure, routing package, HTTP
   client, or serialization approach; those are either separate concerns or deferred to a later
   "Flutter foundation" implementation phase referenced by ADR 0005's migration strategy.

## Alternatives Considered

- **Bloc/Cubit.** Rejected for the current project scale: Bloc's explicit event/state separation
  and associated boilerplate suit larger teams or more complex state machines than this project's
  current feature set (five Blazor pages' worth of equivalent screens, four Application services)
  requires. This is not a rejection of Bloc's quality, only a judgment that it is more structure
  than this project currently needs (per the project's general preference to avoid introducing
  more technology than the current scale justifies).
- **Provider (the package Riverpod succeeded).** Rejected: Riverpod is Provider's intended
  successor, addressing several of Provider's known limitations (compile-time safety, testability
  without a widget tree), with a comparable learning curve; there is no reason to choose the
  predecessor for a new codebase.
- **No state management package; ad hoc `setState`/singletons only.** Rejected: this would push
  authentication state and API loading/error state into ad hoc global mutable state or deeply
  nested widget state, which the project's own experience with Blazor's simple per-component field
  approach shows becomes harder to reason about once a dashboard-like screen aggregates multiple
  async data sources (as `LearnerDashboardService.GetDashboardAsync` already does today).

## Consequences

- No Flutter code exists yet; this ADR constrains future Flutter implementation work but does not
  perform any of it.
- Business logic (quiz scoring, XP/level calculation, lesson completion rules, streak
  advancement) remains entirely server-side in `ManLearning.Domain`/`ManLearning.Application`;
  Riverpod providers in Flutter must only orchestrate calls to `ManLearning.Api` and hold the
  resulting state, never reimplement these rules.
- Choosing Riverpod implies the Flutter project will use `flutter_riverpod` (and, if adopted
  later, Riverpod's code-generation variant); this ADR does not install any package.
- If the project's scale grows enough that Riverpod's simplicity becomes a limitation (for
  example, if the state graph becomes large enough that Bloc's stricter structure would pay for
  itself), this ADR should be superseded by a new one rather than silently reworked in code.

## Migration / Implementation Notes

- The precise Flutter project/directory structure (feature-first vs. layer-first folder layout,
  exact folder names) is explicitly **not decided by this ADR** and is deferred to the "Flutter
  foundation" phase described in ADR 0005, once the project is actually scaffolded.
- This ADR assumes API access from Flutter happens through a Repository-style abstraction that
  Riverpod providers depend on, consistent with ADR 0006 (`ManLearning.Api`) and ADR 0008 (OpenAPI
  contract), but the exact shape of that abstraction is also implementation-phase work.
