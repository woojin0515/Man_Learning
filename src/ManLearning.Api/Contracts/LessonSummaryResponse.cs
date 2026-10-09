using ManLearning.Application.Learning.Dtos;

namespace ManLearning.Api.Contracts;

/// <summary>
/// API-boundary representation of a single lesson as shown in a course's lesson list (
/// <c>GET /api/courses/{courseId}</c>). Deliberately lighter than <see cref="LessonResponse"/>:
/// the list view only needs enough information to render a lesson row and navigate to
/// <c>GET /api/lessons/{lessonId}</c> for full detail, not the quiz's questions/answer choices.
/// </summary>
public sealed record LessonSummaryResponse(Guid Id, string Title, int Position, bool HasQuiz)
{
    public static LessonSummaryResponse FromDto(LessonDto dto) => new(
        dto.Id.Value, dto.Title, dto.Position, dto.Quiz is not null);
}
