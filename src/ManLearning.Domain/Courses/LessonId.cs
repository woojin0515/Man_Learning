namespace ManLearning.Domain.Courses;

public readonly record struct LessonId(Guid Value)
{
    public static LessonId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();
}
