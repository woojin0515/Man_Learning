using ManLearning.Application.Learning;
using ManLearning.Application.Learning.Dtos;
using ManLearning.Application.Tests.TestDoubles;
using ManLearning.Domain.Courses;
using ManLearning.Domain.Learners;
using ManLearning.Domain.Progress;
using ManLearning.Domain.Quizzes;

namespace ManLearning.Application.Tests.Learning;

public class QuizAttemptServiceTests
{
    private sealed record Fixture(
        QuizAttemptService Service,
        InMemoryLessonProgressRepository ProgressRepository,
        InMemoryXpAwardRepository XpAwardRepository,
        Lesson Lesson,
        AnswerChoice CorrectChoice,
        AnswerChoice WrongChoice,
        Question Question);

    private static Fixture CreateFixture()
    {
        var courseRepository = new InMemoryCourseRepository();
        var course = new Course(CourseId.New(), "AI Fundamentals");

        var correctChoice = new AnswerChoice(AnswerChoiceId.New(), "Correct", isCorrect: true);
        var wrongChoice = new AnswerChoice(AnswerChoiceId.New(), "Wrong", isCorrect: false);
        var question = new Question(QuestionId.New(), "What is AI?", [correctChoice, wrongChoice]);
        var quiz = new Quiz(QuizId.New(), [question]);
        var lesson = new Lesson(LessonId.New(), "What is AI?", position: 0, quiz: quiz);
        course.AddLesson(lesson);
        courseRepository.Add(course);

        var progressRepository = new InMemoryLessonProgressRepository();
        var xpAwardRepository = new InMemoryXpAwardRepository();
        var dateTimeProvider = new FixedDateTimeProvider(new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero));

        var service = new QuizAttemptService(
            courseRepository, progressRepository, xpAwardRepository, dateTimeProvider);

        return new Fixture(service, progressRepository, xpAwardRepository, lesson, correctChoice, wrongChoice, question);
    }

    [Fact]
    public async Task SubmitQuizAttemptAsync_WithAllCorrectAnswers_CompletesLessonAndAwardsXp()
    {
        var fixture = CreateFixture();
        var learnerId = LearnerId.New();
        var command = new SubmitQuizAttemptCommand(
            learnerId,
            fixture.Lesson.Id,
            [new QuestionAnswerDto(fixture.Question.Id, fixture.CorrectChoice.Id)]);

        var result = await fixture.Service.SubmitQuizAttemptAsync(command);

        Assert.True(result.LessonCompleted);
        Assert.Equal(LessonCompletionPolicy.LessonCompletionXpAmount, result.XpAwarded);
        Assert.Single(fixture.XpAwardRepository.Awards);

        var progress = await fixture.ProgressRepository.FindAsync(learnerId, fixture.Lesson.Id);
        Assert.Equal(LessonCompletionState.Completed, progress!.State);
    }

    [Fact]
    public async Task SubmitQuizAttemptAsync_WithWrongAnswer_DoesNotCompleteLessonOrAwardXp()
    {
        var fixture = CreateFixture();
        var learnerId = LearnerId.New();
        var command = new SubmitQuizAttemptCommand(
            learnerId,
            fixture.Lesson.Id,
            [new QuestionAnswerDto(fixture.Question.Id, fixture.WrongChoice.Id)]);

        var result = await fixture.Service.SubmitQuizAttemptAsync(command);

        Assert.False(result.LessonCompleted);
        Assert.Equal(0, result.XpAwarded);
        Assert.Empty(fixture.XpAwardRepository.Awards);
    }

    [Fact]
    public async Task SubmitQuizAttemptAsync_WhenAlreadyCompleted_DoesNotAwardXpAgain()
    {
        var fixture = CreateFixture();
        var learnerId = LearnerId.New();
        var command = new SubmitQuizAttemptCommand(
            learnerId,
            fixture.Lesson.Id,
            [new QuestionAnswerDto(fixture.Question.Id, fixture.CorrectChoice.Id)]);

        await fixture.Service.SubmitQuizAttemptAsync(command);
        var secondResult = await fixture.Service.SubmitQuizAttemptAsync(command);

        Assert.True(secondResult.LessonCompleted);
        Assert.Equal(0, secondResult.XpAwarded);
        Assert.Single(fixture.XpAwardRepository.Awards);
    }
}
