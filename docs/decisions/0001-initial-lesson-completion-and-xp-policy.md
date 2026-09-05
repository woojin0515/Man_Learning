# ADR 0001: Initial Lesson Completion and XP Policy

## Status

Accepted (provisional) — scoped to the first vertical slice only.

## Context

The domain model (`docs/architecture/domain-model.md`) intentionally leaves the exact lesson
completion policy, the XP curve, and level thresholds as open decisions. To implement the first
vertical slice (`Course → Lesson → Quiz → Quiz attempt → Lesson completion → XP award`), the
Application layer needs a concrete, working rule today.

## Decision

For the first vertical slice only:

1. A lesson with a quiz is marked **completed** when the learner answers every question
   correctly (`CorrectAnswerCount == TotalQuestionCount`). Partial credit does not complete the
   lesson.
2. A learner is awarded a **flat 10 XP** the first time a lesson is completed. Repeating an
   already-completed lesson's quiz does not award additional XP.
3. Lessons without a quiz are completed through an explicit "start lesson" action only; quiz
   scoring does not apply to them.

This policy lives in `ManLearning.Application.Learning.LessonCompletionPolicy` as a single,
clearly documented place, not scattered across use cases.

## Consequences

- The policy is easy to find and change without touching Domain invariants.
- No difficulty-based XP, streak, or achievement logic exists yet; those remain future work per
  the domain model's deferred decisions.
- When the product requires partial credit, tiered XP, or difficulty-adjusted rewards, this ADR
  should be superseded by a new one rather than silently changed in code.
