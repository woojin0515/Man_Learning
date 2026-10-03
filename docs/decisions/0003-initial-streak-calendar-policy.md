# ADR 0003: Initial Streak Calendar and Advancement Policy

## Status

Accepted (provisional) — scoped to unblocking a visible streak counter, not a final policy.

## Context

The domain model (`docs/architecture/domain-model.md`) defines `Streak` as consecutive learning
days recorded for a learner and states invariant 8: "a streak advances at most once per calendar
day for a learner." It explicitly leaves the calendar/time-zone policy as an open decision.

To show a streak in the Web UI, the Application layer needs a concrete, working rule today
without waiting for a full time-zone/localization design (which depends on the still-undecided
authentication and learner profile model).

## Decision

For this slice only:

1. "Calendar day" means the UTC calendar date (`DateOnly` derived from `IDateTimeProvider.UtcNow`),
   not the learner's local time zone. This matches the existing `XpAward.AwardedAtUtc` convention
   and avoids introducing a time-zone concept before one is designed.
2. A streak advances by exactly one when a learner's most recent recorded activity date is
   exactly one UTC calendar day before today's UTC calendar date.
3. If a learner's most recent recorded activity date is today (UTC), the streak does not change
   (invariant 8: at most once per day).
4. If a learner's most recent recorded activity date is more than one day before today (UTC), or
   there is no prior activity, the streak resets to 1 (a streak of 1 means "active today").
5. A streak is advanced as a side effect of completing a lesson (the same trigger that awards
   XP), via `ManLearning.Domain.Learners.Streak.RecordActivity`. It does not require a separate
   explicit "check in" action in this slice.
6. The domain also tracks the longest streak reached, so the UI can show a personal best without
   a separate read model.

This policy lives in `ManLearning.Domain.Learners.Streak` (the calendar rule and invariant) with
the UTC-day extraction performed by the caller using `IDateTimeProvider`, keeping the Domain free
of `DateTimeOffset.UtcNow` calls.

## Consequences

- Learners in time zones behind UTC may see their streak roll over earlier in their local evening
  than they might expect; this is an accepted limitation until the learner profile/time-zone
  design exists.
- No "grace period" (e.g. forgiving a single missed day) or streak-freeze mechanic exists yet;
  those remain future work.
- When the product requires local time zones, grace periods, or multiple daily activity types
  counting independently, this ADR should be superseded by a new one rather than silently changed
  in code.
