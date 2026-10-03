using ManLearning.Application.Abstractions;
using ManLearning.Application.Learning.Dtos;
using ManLearning.Domain.Learners;
using ManLearning.Domain.Progress;

namespace ManLearning.Application.Learning;

/// <summary>
/// Read-only use case that aggregates a learner's XP and lesson completion into a dashboard
/// snapshot. Deliberately excludes level, streak, and achievement data; those rules are deferred
/// open decisions (see docs/architecture/domain-model.md) and must not be invented here.
/// </summary>
public sealed class LearnerDashboardService(
    ICourseRepository courseRepository,
    ILessonProgressRepository lessonProgressRepository,
    IXpAwardRepository xpAwardRepository)
{
    public async Task<LearnerDashboardDto> GetDashboardAsync(
        LearnerId learnerId, CancellationToken cancellationToken = default)
    {
        var courses = await courseRepository.GetAllAsync(cancellationToken);
        var awards = await xpAwardRepository.GetByLearnerAsync(learnerId, cancellationToken);
        var progressRecords = await lessonProgressRepository.GetAllForLearnerAsync(
            learnerId, cancellationToken);

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

        return new LearnerDashboardDto(
            awards.Sum(award => award.Amount),
            courseProgress.Sum(course => course.CompletedLessonCount),
            courseProgress.Sum(course => course.TotalLessonCount),
            courseProgress);
    }
}
