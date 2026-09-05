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

    Task SaveAsync(LessonProgress progress, CancellationToken cancellationToken = default);
}
