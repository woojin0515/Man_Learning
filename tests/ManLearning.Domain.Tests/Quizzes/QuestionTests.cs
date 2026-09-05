using ManLearning.Domain;
using ManLearning.Domain.Quizzes;

namespace ManLearning.Domain.Tests.Quizzes;

public class QuestionTests
{
    [Fact]
    public void Constructor_WithNoAnswerChoices_ThrowsDomainException()
    {
        Assert.Throws<DomainException>(
            () => new Question(QuestionId.New(), "What is AI?", answerChoices: []));
    }

    [Fact]
    public void Constructor_WithNoCorrectAnswerChoice_ThrowsDomainException()
    {
        var choices = new[]
        {
            new AnswerChoice(AnswerChoiceId.New(), "Wrong 1", isCorrect: false),
            new AnswerChoice(AnswerChoiceId.New(), "Wrong 2", isCorrect: false),
        };

        Assert.Throws<DomainException>(() => new Question(QuestionId.New(), "What is AI?", choices));
    }

    [Fact]
    public void IsCorrectChoice_WithUnknownChoice_ThrowsDomainException()
    {
        var correctChoice = new AnswerChoice(AnswerChoiceId.New(), "Correct", isCorrect: true);
        var question = new Question(QuestionId.New(), "What is AI?", [correctChoice]);

        Assert.Throws<DomainException>(() => question.IsCorrectChoice(AnswerChoiceId.New()));
    }
}
