using ManLearning.Domain.Quizzes;

namespace ManLearning.Application.Learning.Dtos;

/// <summary>
/// A learner's selected answer choice for one question, as submitted from the Web layer.
/// </summary>
public sealed record QuestionAnswerDto(QuestionId QuestionId, AnswerChoiceId SelectedAnswerChoiceId);
