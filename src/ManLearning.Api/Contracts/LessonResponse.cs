using ManLearning.Application.Learning.Dtos;

namespace ManLearning.Api.Contracts;

/// <summary>
/// API-boundary representation of a single lesson for the <c>GET /api/lessons/{lessonId}</c>
/// endpoint. This is a deliberately separate type from both <see cref="Domain.Courses.Lesson"/>
/// and <see cref="LessonDto"/> (ADR-0006's DTO boundary principle).
/// </summary>
/// <remarks>
/// Lesson content/body is not currently modeled in the Domain (see
/// docs/architecture/domain-model.md, "Open decisions"), so this response exposes only the
/// Lesson metadata that already exists: title, position, and quiz structure (if any). Modeling
/// actual lesson content is a separate future architectural decision, not part of this slice.
/// </remarks>
public sealed record LessonResponse(Guid Id, string Title, int Position, QuizResponse? Quiz)
{
    public static LessonResponse FromDto(LessonDto dto) => new(
        dto.Id.Value,
        dto.Title,
        dto.Position,
        dto.Quiz is null ? null : QuizResponse.FromDto(dto.Quiz));
}
