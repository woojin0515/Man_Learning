namespace ManLearning.Domain.Quizzes;

public readonly record struct QuizId(Guid Value)
{
    public static QuizId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();
}
