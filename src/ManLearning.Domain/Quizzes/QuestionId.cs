namespace ManLearning.Domain.Quizzes;

public readonly record struct QuestionId(Guid Value)
{
    public static QuestionId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();
}
