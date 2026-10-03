using ManLearning.Domain.Courses;
using ManLearning.Domain.Learners;
using ManLearning.Domain.Progress;

namespace ManLearning.Application.Abstractions;

/// <summary>
/// Persistence abstraction for a learner's lesson completion state.
/// </summary>
public interface ILessonProgressRepository
{
    Task<LessonProgress?> FindAsync(
        LearnerId learnerId, LessonId lessonId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns every lesson progress record for a learner, used to compute dashboard summaries
    /// (for example, completed lesson counts per course).
    /// </summary>
    Task<IReadOnlyList<LessonProgress>> GetAllForLearnerAsync(
        LearnerId learnerId, CancellationToken cancellationToken = default);

    Task SaveAsync(LessonProgress progress, CancellationToken cancellationToken = default);
}
