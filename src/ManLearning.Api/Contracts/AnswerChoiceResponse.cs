using ManLearning.Application.Learning.Dtos;

namespace ManLearning.Api.Contracts;

/// <summary>
/// API-boundary representation of a single answer choice for <c>GET /api/lessons/{lessonId}</c>.
/// Deliberately omits whether the choice is correct (mirrors <see cref="AnswerChoiceDto"/>'s own
/// boundary, which already excludes it) so the API never leaks answer keys to Flutter.
/// </summary>
public sealed record AnswerChoiceResponse(Guid Id, string Text)
{
    public static AnswerChoiceResponse FromDto(AnswerChoiceDto dto) => new(dto.Id.Value, dto.Text);
}
