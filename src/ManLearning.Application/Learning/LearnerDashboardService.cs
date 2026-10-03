using ManLearning.Application.Abstractions;
using ManLearning.Application.Learning.Dtos;
using ManLearning.Domain.Learners;
using ManLearning.Domain.Progress;
using ManLearning.Domain.Xp;

namespace ManLearning.Application.Learning;

/// <summary>
/// Read-only use case that aggregates a learner's XP, level, streak, and lesson completion into a
/// dashboard snapshot. Deliberately excludes achievement data; the achievement catalog remains a
/// deferred open decision (see docs/architecture/domain-model.md) and must not be invented here.
/// </summary>
public sealed class LearnerDashboardService(
    ICourseRepository courseRepository,
    ILessonProgressRepository lessonProgressRepository,
    IXpAwardRepository xpAwardRepository,
    IStreakRepository streakRepository)
{
    public async Task<LearnerDashboardDto> GetDashboardAsync(
        LearnerId learnerId, CancellationToken cancellationToken = default)
    {
        var courses = await courseRepository.GetAllAsync(cancellationToken);
        var awards = await xpAwardRepository.GetByLearnerAsync(learnerId, cancellationToken);
        var progressRecords = await lessonProgressRepository.GetAllForLearnerAsync(
            learnerId, cancellationToken);
        var streak = await streakRepository.FindAsync(learnerId, cancellationToken)
            ?? new Streak(learnerId);

        var completedLessonIds = progressRecords
            .Where(progress => progress.State == LessonCompletionState.Completed)
            .Select(progress => progress.LessonId)
            .ToHashSet();

        var courseProgress = courses
            .Select(course => new CourseProgressDto(
                course.Id,
                course.Title,
                course.Lessons.Count(lesson => completedLessonIds.Contains(lesson.Id)),
                course.Lessons.Count))
            .ToList();

        var totalXp = awards.Sum(award => award.Amount);
        var levelProgress = LevelCurve.GetLevelProgress(totalXp);

        return new LearnerDashboardDto(
            totalXp,
            new LevelProgressDto(
                levelProgress.Level,
                levelProgress.CurrentLevelXp,
                levelProgress.NextLevelXp,
                levelProgress.ProgressToNextLevel),
            new StreakDto(streak.CurrentLength, streak.LongestLength, streak.LastActiveDate),
            courseProgress.Sum(course => course.CompletedLessonCount),
            courseProgress.Sum(course => course.TotalLessonCount),
            courseProgress);
    }
}
