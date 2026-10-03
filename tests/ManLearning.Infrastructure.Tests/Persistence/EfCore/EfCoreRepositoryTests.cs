using ManLearning.Domain.Courses;
using ManLearning.Domain.Learners;
using ManLearning.Domain.Progress;
using ManLearning.Domain.Quizzes;
using ManLearning.Infrastructure.Persistence.EfCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ManLearning.Infrastructure.Tests.Persistence.EfCore;

/// <summary>
/// Round-trip tests proving the EF Core mapping in
/// <c>src/ManLearning.Infrastructure/Persistence/EfCore/Configurations</c> actually persists and
/// restores the full Course aggregate (Lessons, owned Quiz/Question/AnswerChoice) and the other
/// repositories, against a real relational provider (SQLite). ADR 0004
/// (docs/decisions/0004-database-technology.md) targets Azure SQL Database in production, but the
/// mapping itself is provider-agnostic, so SQLite is a fast, no-install stand-in for these tests.
/// </summary>
public sealed class EfCoreRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ManLearningDbContext _dbContext;

    public EfCoreRepositoryTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<ManLearningDbContext>()
            .UseSqlite(_connection)
            .Options;

        _dbContext = new ManLearningDbContext(options);
        _dbContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        _connection.Dispose();
    }

    [Fact]
    public async Task CourseRepository_RoundTrips_FullAggregate()
    {
        var course = BuildCourseWithQuiz();
        _dbContext.Courses.Add(course);
        await _dbContext.SaveChangesAsync();

        // Use a fresh context so the assertions read from the database, not the change tracker.
        await using var readContext = CreateFreshContext();
        var repository = new EfCoreCourseRepository(readContext);

        var loaded = await repository.GetByIdAsync(course.Id);

        Assert.NotNull(loaded);
        Assert.Equal(course.Title, loaded!.Title);
        Assert.Single(loaded.Lessons);

        var lesson = loaded.Lessons[0];
        Assert.Equal("Lesson 1", lesson.Title);
        Assert.NotNull(lesson.Quiz);
        Assert.Single(lesson.Quiz!.Questions);

        var question = lesson.Quiz.Questions[0];
        Assert.Equal(2, question.AnswerChoices.Count);
        Assert.Contains(question.AnswerChoices, choice => choice.IsCorrect);
    }

    [Fact]
    public async Task LessonProgressRepository_SaveAsync_InsertsThenUpdates()
    {
        var learnerId = LearnerId.New();
        var lessonId = LessonId.New();
        var repository = new EfCoreLessonProgressRepository(_dbContext);

        var progress = new LessonProgress(learnerId, lessonId);
        progress.Start();
        await repository.SaveAsync(progress);

        var reloaded = await repository.FindAsync(learnerId, lessonId);
        Assert.NotNull(reloaded);
        Assert.Equal(LessonCompletionState.InProgress, reloaded!.State);

        reloaded.Complete();
        await repository.SaveAsync(reloaded);

        await using var readContext = CreateFreshContext();
        var verifyRepository = new EfCoreLessonProgressRepository(readContext);
        var completed = await verifyRepository.FindAsync(learnerId, lessonId);

        Assert.NotNull(completed);
        Assert.Equal(LessonCompletionState.Completed, completed!.State);
    }

    private ManLearningDbContext CreateFreshContext()
    {
        var options = new DbContextOptionsBuilder<ManLearningDbContext>()
            .UseSqlite(_connection)
            .Options;

        return new ManLearningDbContext(options);
    }

    private static Course BuildCourseWithQuiz()
    {
        var course = new Course(CourseId.New(), "AI Fundamentals");

        var correctChoice = new AnswerChoice(AnswerChoiceId.New(), "Correct answer", isCorrect: true);
        var wrongChoice = new AnswerChoice(AnswerChoiceId.New(), "Wrong answer", isCorrect: false);
        var question = new Question(QuestionId.New(), "What is AI?", [correctChoice, wrongChoice]);
        var quiz = new Quiz(QuizId.New(), [question]);

        var lesson = new Lesson(LessonId.New(), "Lesson 1", position: 0, quiz);
        course.AddLesson(lesson);

        return course;
    }
}
