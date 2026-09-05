using ManLearning.Domain.Courses;

namespace ManLearning.Application.Abstractions;

/// <summary>
/// Persistence abstraction for courses and their lessons. The concrete storage technology is a
/// deferred technical spike (see docs/architecture/domain-model.md); Application only depends on
/// this interface.
/// </summary>
public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(CourseId courseId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds a single lesson (and its quiz, if any) without requiring the caller to load and
    /// search the full course aggregate.
    /// </summary>
    Task<Lesson?> FindLessonAsync(LessonId lessonId, CancellationToken cancellationToken = default);
}
