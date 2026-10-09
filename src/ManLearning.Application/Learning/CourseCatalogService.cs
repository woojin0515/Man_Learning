using ManLearning.Application.Abstractions;
using ManLearning.Application.Common;
using ManLearning.Application.Learning.Dtos;
using ManLearning.Domain.Courses;
using ManLearning.Domain.Quizzes;

namespace ManLearning.Application.Learning;

/// <summary>
/// Read-only use case for browsing course content. Maps Domain aggregates to DTOs so that the
/// Web layer never depends on Domain types directly.
/// </summary>
public sealed class CourseCatalogService(ICourseRepository courseRepository)
{
    public async Task<IReadOnlyList<CourseSummaryDto>> GetCourseListAsync(
        CancellationToken cancellationToken = default)
    {
        var courses = await courseRepository.GetAllAsync(cancellationToken);

        return [.. courses.Select(course => new CourseSummaryDto(
            course.Id, course.Title, course.Lessons.Count))];
    }

    public async Task<CourseDto> GetCourseAsync(
        CourseId courseId, CancellationToken cancellationToken = default)
    {
        var course = await courseRepository.GetByIdAsync(courseId, cancellationToken)
            ?? throw new NotFoundException($"Course {courseId} was not found.");

        return MapToDto(course);
    }

    /// <summary>
    /// Looks up a single lesson (and its quiz, if any) without loading the owning course's full
    /// lesson list — the Lesson Detail vertical slice only needs this one lesson.
    /// </summary>
    public async Task<LessonDto> GetLessonAsync(
        LessonId lessonId, CancellationToken cancellationToken = default)
    {
        var lesson = await courseRepository.FindLessonAsync(lessonId, cancellationToken)
            ?? throw new NotFoundException($"Lesson {lessonId} was not found.");

        return MapToDto(lesson);
    }

    private static CourseDto MapToDto(Course course) => new(
        course.Id,
        course.Title,
        [.. course.Lessons.OrderBy(lesson => lesson.Position).Select(MapToDto)]);

    private static LessonDto MapToDto(Lesson lesson) => new(
        lesson.Id,
        lesson.Title,
        lesson.Position,
        lesson.Quiz is null ? null : MapToDto(lesson.Quiz));

    private static QuizDto MapToDto(Quiz quiz) => new(
        quiz.Id,
        [.. quiz.Questions.Select(MapToDto)]);

    private static QuestionDto MapToDto(Question question) => new(
        question.Id,
        question.Text,
        [.. question.AnswerChoices.Select(choice => new AnswerChoiceDto(choice.Id, choice.Text))]);
}
