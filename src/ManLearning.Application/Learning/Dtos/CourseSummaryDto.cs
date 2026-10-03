using ManLearning.Domain.Courses;

namespace ManLearning.Application.Learning.Dtos;

/// <summary>
/// A lightweight projection of a course for catalog/list views, avoiding the cost of loading
/// every lesson's full quiz definition.
/// </summary>
public sealed record CourseSummaryDto(CourseId Id, string Title, int LessonCount);
