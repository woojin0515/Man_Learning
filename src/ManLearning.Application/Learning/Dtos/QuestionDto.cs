using ManLearning.Domain.Quizzes;

namespace ManLearning.Application.Learning.Dtos;

public sealed record QuestionDto(QuestionId Id, string Text, IReadOnlyList<AnswerChoiceDto> AnswerChoices);
