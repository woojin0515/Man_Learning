using ManLearning.Domain.Courses;

namespace ManLearning.Application.Learning.Dtos;

/// <summary>
/// A learner's completion progress within a single course, for dashboard display.
/// </summary>
public sealed record CourseProgressDto(
    CourseId CourseId, string CourseTitle, int CompletedLessonCount, int TotalLessonCount);
