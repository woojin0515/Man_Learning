namespace ManLearning.Domain.Quizzes;

public readonly record struct AnswerChoiceId(Guid Value)
{
    public static AnswerChoiceId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();
}
