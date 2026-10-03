namespace ManLearning.Application.Learning.Dtos;

/// <summary>
/// A learner's level and progress toward the next level, projected from
/// <see cref="Domain.Xp.LevelProgress"/> so the Web layer never depends on Domain types directly.
/// See ADR 0002 (docs/decisions/0002-initial-level-curve.md) for the curve this is based on.
/// </summary>
public sealed record LevelProgressDto(
    int Level, int CurrentLevelXp, int NextLevelXp, double ProgressToNextLevel);
