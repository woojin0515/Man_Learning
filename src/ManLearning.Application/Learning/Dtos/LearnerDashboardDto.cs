namespace ManLearning.Application.Learning.Dtos;

/// <summary>
/// A learner's overall progress snapshot: total XP earned, level, streak, and lesson completion
/// across every course. Achievement data is not included here; the achievement catalog remains a
/// deferred open decision (see "Open decisions for later work" in
/// docs/architecture/domain-model.md).
/// </summary>
public sealed record LearnerDashboardDto(
    int TotalXp,
    LevelProgressDto LevelProgress,
    StreakDto Streak,
    int CompletedLessonCount,
    int TotalLessonCount,
    IReadOnlyList<CourseProgressDto> CourseProgress);
