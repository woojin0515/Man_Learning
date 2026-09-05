using ManLearning.Domain.Quizzes;

namespace ManLearning.Application.Learning.Dtos;

/// <summary>
/// A selectable quiz answer as shown to a learner. Deliberately omits whether the choice is
/// correct so that Application never leaks answer keys to the Web layer.
/// </summary>
public sealed record AnswerChoiceDto(AnswerChoiceId Id, string Text);
