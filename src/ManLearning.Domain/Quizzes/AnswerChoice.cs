namespace ManLearning.Domain.Quizzes;

/// <summary>
/// A selectable response to a question. Correctness is a domain property, not a UI concern.
/// </summary>
public sealed class AnswerChoice
{
    public AnswerChoiceId Id { get; }
    public string Text { get; }
    public bool IsCorrect { get; }

    public AnswerChoice(AnswerChoiceId id, string text, bool isCorrect)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new DomainException("Answer choice text cannot be empty.");
        }

        Id = id;
        Text = text;
        IsCorrect = isCorrect;
    }
}
