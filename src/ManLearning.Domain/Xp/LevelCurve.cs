namespace ManLearning.Domain.Xp;

/// <summary>
/// A learner's level and progress toward the next level, derived from total XP. See ADR 0002
/// (docs/decisions/0002-initial-level-curve.md) for the curve this is based on.
/// </summary>
/// <param name="Level">The learner's current level. Always at least 1.</param>
/// <param name="CurrentLevelXp">Total XP required to have reached <paramref name="Level"/>.</param>
/// <param name="NextLevelXp">Total XP required to reach the next level.</param>
/// <param name="ProgressToNextLevel">
/// Progress toward the next level, in the range [0, 1].
/// </param>
public sealed record LevelProgress(
    int Level, int CurrentLevelXp, int NextLevelXp, double ProgressToNextLevel);

/// <summary>
/// Converts a learner's total XP into a level and progress toward the next level. See ADR 0002
/// (docs/decisions/0002-initial-level-curve.md): level N requires 100 * N cumulative XP, and a
/// learner with 0 XP is Level 1 (there is no "no level" state).
/// </summary>
public static class LevelCurve
{
    private const int XpPerLevel = 100;

    public static LevelProgress GetLevelProgress(int totalXp)
    {
        if (totalXp < 0)
        {
            throw new DomainException("Total XP cannot be negative.");
        }

        var level = totalXp / XpPerLevel + 1;
        var currentLevelXp = (level - 1) * XpPerLevel;
        var nextLevelXp = level * XpPerLevel;

        var progress = (double)(totalXp - currentLevelXp) / (nextLevelXp - currentLevelXp);

        return new LevelProgress(level, currentLevelXp, nextLevelXp, progress);
    }
}
