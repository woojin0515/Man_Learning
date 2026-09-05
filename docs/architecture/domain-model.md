# Initial Learning Domain Model

## Scope

This document defines the initial domain vocabulary for Man Learning. It is a discovery artifact, not a database schema or UI specification. Provider choices, persistence details, authentication, and AI integrations remain outside this scope.

## Core concepts

| Concept | Responsibility |
| --- | --- |
| Course | An ordered learning path made up of lessons. |
| Lesson | A short unit of instructional content within a course. |
| Quiz | An assessment attached to a lesson or learning unit. |
| Question | A single prompt in a quiz with one or more answer choices. |
| Answer choice | A selectable response; correctness is a domain property, not a UI concern. |
| Learner progress | A learner's completion state for a course or lesson. |
| Quiz attempt | A learner's submitted answers and resulting score for a quiz. |
| XP award | A domain record of experience earned from a completed learning action. |
| Level | A progression state derived from accumulated XP. |
| Streak | Consecutive learning days recorded for a learner. |
| Achievement | A named milestone awarded when its conditions are satisfied. |

## Initial boundaries

- Course ordering and lesson ordering are part of the learning domain.
- A lesson can be completed only through the completion rule defined by the lesson type.
- Quiz scoring belongs to the domain and must not depend on UI state or persistence.
- XP, level, streak, and achievement rules are domain behavior; display formatting is not.
- Learner identity is represented as an opaque identifier until authentication is designed.
- Persistence, external services, and provider-specific models do not belong in Domain.

## Initial invariants

1. Course and lesson titles cannot be empty.
2. A course cannot contain duplicate lesson positions.
3. A quiz cannot contain a question without at least one answer choice.
4. A question must have a valid answer choice selected before an attempt can be scored.
5. A quiz score is calculated from submitted answers and the quiz definition, not from a client-provided score.
6. Progress cannot move backward through a completion state.
7. XP is awarded through explicit domain actions and cannot be negative.
8. A streak advances at most once per calendar day for a learner.
9. An achievement is awarded at most once for a learner.
10. Domain objects do not reference ASP.NET Core, Blazor, database providers, or external API SDKs.

## First vertical slice

The first implementation slice should be:

`Course → Lesson → Quiz → Quiz attempt → Lesson completion → XP award`

The slice should establish the domain language and scoring/completion rules before adding levels, streaks, or achievements. Application use cases and persistence should be designed against the resulting domain behavior rather than exposing storage models directly.

## Open decisions for later work

- Whether course content is authored in code, files, or a persistence store.
- Whether a quiz supports multiple correct answers, free text, or only single-choice questions.
- The exact lesson completion policy.
- The XP curve and level thresholds.
- The calendar/time-zone policy for streaks.
- The achievement catalog and evaluation timing.
- The learner identity and authentication model.
