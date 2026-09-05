using ManLearning.Application.Abstractions;
using ManLearning.Domain.Courses;

namespace ManLearning.Application.Tests.TestDoubles;

/// <summary>
/// Minimal in-memory test double for <see cref="ICourseRepository"/>. Avoids pulling in a
/// mocking library for a handful of simple use-case tests.
/// </summary>
internal sealed class InMemoryCourseRepository : ICourseRepository
{
    private readonly List<Course> _courses = [];

    public void Add(Course course) => _courses.Add(course);

    public Task<Course?> GetByIdAsync(CourseId courseId, CancellationToken cancellationToken = default) =>
        Task.FromResult(_courses.FirstOrDefault(course => course.Id == courseId));

    public Task<Lesson?> FindLessonAsync(LessonId lessonId, CancellationToken cancellationToken = default) =>
        Task.FromResult(_courses
            .SelectMany(course => course.Lessons)
            .FirstOrDefault(lesson => lesson.Id == lessonId));
}
