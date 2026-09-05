using ManLearning.Domain.Courses;

namespace ManLearning.Application.Learning.Dtos;

public sealed record LessonDto(LessonId Id, string Title, int Position, QuizDto? Quiz);
