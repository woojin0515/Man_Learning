namespace ManLearning.Application.Learning.Dtos;

/// <summary>
/// A learner's overall progress snapshot: total XP earned and lesson completion across every
/// course. Level, streak, and achievement data are not included here; those rules are deferred
/// open decisions (see "Open decisions for later work" in docs/architecture/domain-model.md).
/// </summary>
public sealed record LearnerDashboardDto(
    int TotalXp,
    LevelProgressDto LevelProgress,
    int CompletedLessonCount,
    int TotalLessonCount,
    IReadOnlyList<CourseProgressDto> CourseProgress);
