using ManLearning.Domain.Quizzes;

namespace ManLearning.Application.Learning.Dtos;

public sealed record QuizDto(QuizId Id, IReadOnlyList<QuestionDto> Questions);
