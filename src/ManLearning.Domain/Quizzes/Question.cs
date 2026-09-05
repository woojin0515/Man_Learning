namespace ManLearning.Domain.Quizzes;

/// <summary>
/// A single prompt in a quiz. See invariant 3 in docs/architecture/domain-model.md: a quiz
/// cannot contain a question without at least one answer choice.
/// </summary>
public sealed class Question
{
    private readonly List<AnswerChoice> _answerChoices;

    public QuestionId Id { get; }
    public string Text { get; }
    public IReadOnlyList<AnswerChoice> AnswerChoices => _answerChoices;

    public Question(QuestionId id, string text, IReadOnlyList<AnswerChoice> answerChoices)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new DomainException("Question text cannot be empty.");
        }

        if (answerChoices is null || answerChoices.Count == 0)
        {
            throw new DomainException("A question must have at least one answer choice.");
        }

        if (!answerChoices.Any(choice => choice.IsCorrect))
        {
            throw new DomainException("A question must have at least one correct answer choice.");
        }

        Id = id;
        Text = text;
        _answerChoices = [.. answerChoices];
    }

    /// <summary>
    /// Determines whether the given answer choice is the correct response to this question.
    /// </summary>
    public bool IsCorrectChoice(AnswerChoiceId answerChoiceId)
    {
        var choice = _answerChoices.FirstOrDefault(choice => choice.Id == answerChoiceId)
            ?? throw new DomainException(
                $"Answer choice {answerChoiceId} does not belong to question {Id}.");

        return choice.IsCorrect;
    }
}
