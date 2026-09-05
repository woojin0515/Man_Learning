namespace ManLearning.Domain.Quizzes;

/// <summary>
/// A learner's selected answer choice for a single question, submitted as part of a quiz
/// attempt. Scoring is always computed by the domain from these submissions and the quiz
/// definition (see invariant 5 in docs/architecture/domain-model.md) — a client-provided
/// score is never trusted.
/// </summary>
public sealed record QuizAnswerSubmission(QuestionId QuestionId, AnswerChoiceId SelectedAnswerChoiceId);
