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
    public async Task<CourseDto> GetCourseAsync(
        CourseId courseId, CancellationToken cancellationToken = default)
    {
        var course = await courseRepository.GetByIdAsync(courseId, cancellationToken)
            ?? throw new NotFoundException($"Course {courseId} was not found.");

        return MapToDto(course);
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
