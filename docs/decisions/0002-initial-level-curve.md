# ADR 0002: Initial Level Curve

## Status

Accepted (provisional) — scoped to the first level/XP display slice only.

## Context

The domain model (`docs/architecture/domain-model.md`) defines Level as "a progression state
derived from accumulated XP" but defers the exact XP curve and level thresholds as an open
decision. ADR 0001 established a flat 10 XP award per first-time lesson completion. The learner
dashboard (`/me`) now shows total XP; the next step is to translate that XP total into a Level
the learner can see progress toward, without inventing streak or achievement rules that remain
out of scope.

## Decision

For the first level slice only:

1. Level uses a simple arithmetic curve: **Level N requires `100 * N` cumulative XP** to reach
   (Level 1 = 0 XP, Level 2 = 100 XP, Level 3 = 200 XP, and so on). This is deliberately linear,
   not exponential, so it is trivial to reason about and to change once real usage data exists.
2. A learner's current level is the highest level whose threshold their total XP meets or
   exceeds. A learner with 0 XP is Level 1 (not Level 0); there is no "no level" state.
3. Progress toward the next level is exposed as `(CurrentXp - CurrentLevelThreshold) /
   (NextLevelThreshold - CurrentLevelThreshold)`, a value in `[0, 1]`, so the Web layer can
   render a progress bar without re-deriving the curve.
4. This logic lives in a single `ManLearning.Domain.Xp.LevelCurve` type, not scattered across
   Application services, so the curve can be replaced by changing one file.

## Consequences

- The curve is intentionally simple and almost certainly "wrong" for long-term balancing; it
  exists to unblock showing *a* level, not to be the final tuned progression.
- No level-up celebration/animation, streak, or achievement logic exists yet; those remain
  future work per the domain model's deferred decisions.
- When the product requires a non-linear curve, difficulty-scaled thresholds, or a level cap,
  this ADR should be superseded by a new one rather than silently changed in code.
