using ManLearning.Application.Learning.Dtos;

namespace ManLearning.Api.Contracts;

/// <summary>
/// API-boundary representation of a course for the <c>GET /api/courses</c> list endpoint. This is
/// deliberately a separate type from both <see cref="Domain.Courses.Course"/> and
/// <see cref="CourseSummaryDto"/> (ADR-0006's DTO boundary principle): Application's DTOs may
/// still change shape independently of what the API contract promises to Flutter.
/// </summary>
public sealed record CourseResponse(Guid Id, string Title, int LessonCount)
{
    public static CourseResponse FromDto(CourseSummaryDto dto) => new(dto.Id.Value, dto.Title, dto.LessonCount);
}
