using ManLearning.Application.Learning.Dtos;

namespace ManLearning.Api.Contracts;

/// <summary>
/// API-boundary representation of a single quiz question for <c>GET /api/lessons/{lessonId}</c>.
/// </summary>
public sealed record QuestionResponse(Guid Id, string Text, IReadOnlyList<AnswerChoiceResponse> AnswerChoices)
{
    public static QuestionResponse FromDto(QuestionDto dto) => new(
        dto.Id.Value,
        dto.Text,
        [.. dto.AnswerChoices.Select(AnswerChoiceResponse.FromDto)]);
}
