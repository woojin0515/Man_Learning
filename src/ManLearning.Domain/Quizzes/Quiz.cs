namespace ManLearning.Domain.Quizzes;

/// <summary>
/// An assessment attached to a lesson or learning unit. Scoring is a domain responsibility and
/// must not depend on UI state or persistence (see invariant 5 in
/// docs/architecture/domain-model.md).
/// </summary>
public sealed class Quiz
{
    private readonly List<Question> _questions;

    public QuizId Id { get; }
    public IReadOnlyList<Question> Questions => _questions;

    public Quiz(QuizId id, IReadOnlyList<Question> questions)
    {
        if (questions is null || questions.Count == 0)
        {
            throw new DomainException("A quiz must have at least one question.");
        }

        Id = id;
        _questions = [.. questions];
    }

    /// <summary>
    /// Scores a set of submitted answers against this quiz's definition. Every question must
    /// have a corresponding submission (invariant 4).
    /// </summary>
    public QuizAttemptResult Score(IReadOnlyCollection<QuizAnswerSubmission> submissions)
    {
        ArgumentNullException.ThrowIfNull(submissions);

        var submissionsByQuestion = submissions.ToDictionary(submission => submission.QuestionId);

        var correctCount = 0;

        foreach (var question in _questions)
        {
            if (!submissionsByQuestion.TryGetValue(question.Id, out var submission))
            {
                throw new DomainException(
                    $"Missing answer submission for question {question.Id} in quiz {Id}.");
            }

            if (question.IsCorrectChoice(submission.SelectedAnswerChoiceId))
            {
                correctCount++;
            }
        }

        return new QuizAttemptResult(Id, correctCount, _questions.Count);
    }
}
