using ManLearning.Domain.Courses;

namespace ManLearning.Application.Learning.Dtos;

public sealed record CourseDto(CourseId Id, string Title, IReadOnlyList<LessonDto> Lessons);
