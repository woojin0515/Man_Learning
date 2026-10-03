using ManLearning.Domain.Courses;
using ManLearning.Domain.Quizzes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ManLearning.Infrastructure.Persistence.EfCore.Configurations;

/// <summary>
/// Maps <see cref="Course"/> and its full aggregate (Lessons, each lesson's optional Quiz, its
/// Questions, and their AnswerChoices). Quiz/Question/AnswerChoice are configured as owned types
/// because the domain only ever reaches them through a Lesson (see
/// <see cref="ManLearning.Application.Learning.QuizAttemptService"/>), so they do not need
/// independent repository access.
/// </summary>
internal sealed class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> course)
    {
        course.ToTable("Courses");
        course.HasKey(c => c.Id);
        course.Property(c => c.Id).HasConversion(IdValueConverters.CourseId).ValueGeneratedNever();
        course.Property(c => c.Title).IsRequired().HasMaxLength(200);

        course.HasMany(c => c.Lessons)
            .WithOne()
            .HasForeignKey("CourseId")
            .IsRequired();

        course.Metadata.FindNavigation(nameof(Course.Lessons))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}

internal sealed class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> lesson)
    {
        lesson.ToTable("Lessons");
        lesson.HasKey(l => l.Id);
        lesson.Property(l => l.Id).HasConversion(IdValueConverters.LessonId).ValueGeneratedNever();
        lesson.Property(l => l.Title).IsRequired().HasMaxLength(200);
        lesson.Property(l => l.Position).IsRequired();
        lesson.Property<CourseId>("CourseId").HasConversion(IdValueConverters.CourseId);
        lesson.HasIndex("CourseId", nameof(Lesson.Position)).IsUnique();

        var quiz = lesson.OwnsOne(l => l.Quiz);
        quiz.ToTable("Quizzes");
        quiz.WithOwner().HasForeignKey("LessonId");
        quiz.HasKey("LessonId");
        quiz.Property(q => q.Id).HasConversion(IdValueConverters.QuizId);

        var question = quiz.OwnsMany(q => q.Questions);
        question.ToTable("Questions");
        question.WithOwner().HasForeignKey("LessonId");
        question.Property<LessonId>("LessonId").HasConversion(IdValueConverters.LessonId);
        question.HasKey("LessonId", nameof(Question.Id));
        question.Property(q => q.Id).HasConversion(IdValueConverters.QuestionId);
        question.Property(q => q.Text).IsRequired().HasMaxLength(1000);

        var answerChoice = question.OwnsMany(q => q.AnswerChoices);
        answerChoice.ToTable("AnswerChoices");
        answerChoice.WithOwner().HasForeignKey("LessonId", "QuestionId");
        answerChoice.Property<LessonId>("LessonId").HasConversion(IdValueConverters.LessonId);
        answerChoice.Property<QuestionId>("QuestionId").HasConversion(IdValueConverters.QuestionId);
        answerChoice.HasKey("LessonId", "QuestionId", nameof(AnswerChoice.Id));
        answerChoice.Property(c => c.Id).HasConversion(IdValueConverters.AnswerChoiceId);
        answerChoice.Property(c => c.Text).IsRequired().HasMaxLength(500);
        answerChoice.Property(c => c.IsCorrect).IsRequired();

        question.OwnedEntityType.FindNavigation(nameof(Question.AnswerChoices))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);

        quiz.OwnedEntityType.FindNavigation(nameof(Quiz.Questions))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
