using System.Collections.Concurrent;
using ManLearning.Application.Abstractions;
using ManLearning.Domain.Courses;

namespace ManLearning.Infrastructure.Persistence;

/// <summary>
/// In-memory <see cref="ICourseRepository"/> seeded with demo content. This is a temporary
/// stand-in for real persistence; the database technology is a deferred technical spike (see
/// docs/architecture/domain-model.md) and must not be assumed from this implementation.
/// </summary>
public sealed class InMemoryCourseRepository : ICourseRepository
{
    private readonly ConcurrentDictionary<CourseId, Course> _coursesById;

    public InMemoryCourseRepository()
    {
        _coursesById = new ConcurrentDictionary<CourseId, Course>(
            SeedCourseData.BuildCourses().ToDictionary(course => course.Id));
    }

    public Task<IReadOnlyList<Course>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<Course>>([.. _coursesById.Values]);

    public Task<Course?> GetByIdAsync(CourseId courseId, CancellationToken cancellationToken = default)
    {
        _coursesById.TryGetValue(courseId, out var course);
        return Task.FromResult(course);
    }

    public Task<Lesson?> FindLessonAsync(LessonId lessonId, CancellationToken cancellationToken = default)
    {
        var lesson = _coursesById.Values
            .SelectMany(course => course.Lessons)
            .FirstOrDefault(lesson => lesson.Id == lessonId);

        return Task.FromResult(lesson);
    }
}
