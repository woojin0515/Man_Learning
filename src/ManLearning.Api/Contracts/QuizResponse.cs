using ManLearning.Application.Learning.Dtos;

namespace ManLearning.Api.Contracts;

/// <summary>
/// API-boundary representation of a lesson's quiz for <c>GET /api/lessons/{lessonId}</c>.
/// </summary>
public sealed record QuizResponse(Guid Id, IReadOnlyList<QuestionResponse> Questions)
{
    public static QuizResponse FromDto(QuizDto dto) => new(
        dto.Id.Value,
        [.. dto.Questions.Select(QuestionResponse.FromDto)]);
}
