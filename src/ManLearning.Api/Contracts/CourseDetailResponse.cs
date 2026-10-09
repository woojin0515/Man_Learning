using ManLearning.Application.Learning.Dtos;

namespace ManLearning.Api.Contracts;

/// <summary>
/// API-boundary representation of a single course for the <c>GET /api/courses/{courseId}</c>
/// endpoint. This is deliberately a separate type from both <see cref="Domain.Courses.Course"/>
/// and <see cref="CourseDto"/> (ADR-0006's DTO boundary principle, the same one
/// <see cref="CourseResponse"/> already follows for the course list endpoint).
/// </summary>
public sealed record CourseDetailResponse(Guid Id, string Title, IReadOnlyList<LessonSummaryResponse> Lessons)
{
    public static CourseDetailResponse FromDto(CourseDto dto) => new(
        dto.Id.Value,
        dto.Title,
        [.. dto.Lessons.Select(LessonSummaryResponse.FromDto)]);
}
