using ManLearning.Domain.Courses;
using ManLearning.Domain.Learners;
using ManLearning.Domain.Quizzes;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ManLearning.Infrastructure.Persistence.EfCore;

/// <summary>
/// Shared EF Core <see cref="ValueConverter{TModel,TProvider}"/> instances for the opaque
/// <c>Guid</c>-backed identifier types used throughout Domain. Centralizing these here keeps the
/// conversion in one place instead of repeating a lambda pair in every entity configuration.
/// </summary>
internal static class IdValueConverters
{
    public static readonly ValueConverter<CourseId, Guid> CourseId =
        new(id => id.Value, value => new CourseId(value));

    public static readonly ValueConverter<LessonId, Guid> LessonId =
        new(id => id.Value, value => new LessonId(value));

    public static readonly ValueConverter<QuizId, Guid> QuizId =
        new(id => id.Value, value => new QuizId(value));

    public static readonly ValueConverter<QuestionId, Guid> QuestionId =
        new(id => id.Value, value => new QuestionId(value));

    public static readonly ValueConverter<AnswerChoiceId, Guid> AnswerChoiceId =
        new(id => id.Value, value => new AnswerChoiceId(value));

    public static readonly ValueConverter<LearnerId, Guid> LearnerId =
        new(id => id.Value, value => new LearnerId(value));
}
