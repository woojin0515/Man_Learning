using ManLearning.Domain;
using ManLearning.Domain.Quizzes;

namespace ManLearning.Domain.Tests.Quizzes;

public class QuizTests
{
    private static (Quiz Quiz, Question Question, AnswerChoice Correct, AnswerChoice Wrong) CreateSingleQuestionQuiz()
    {
        var correct = new AnswerChoice(AnswerChoiceId.New(), "Correct", isCorrect: true);
        var wrong = new AnswerChoice(AnswerChoiceId.New(), "Wrong", isCorrect: false);
        var question = new Question(QuestionId.New(), "What is AI?", [correct, wrong]);
        var quiz = new Quiz(QuizId.New(), [question]);

        return (quiz, question, correct, wrong);
    }

    [Fact]
    public void Constructor_WithNoQuestions_ThrowsDomainException()
    {
        Assert.Throws<DomainException>(() => new Quiz(QuizId.New(), []));
    }

    [Fact]
    public void Score_WithAllCorrectAnswers_ReturnsFullScore()
    {
        var (quiz, question, correct, _) = CreateSingleQuestionQuiz();

        var result = quiz.Score([new QuizAnswerSubmission(question.Id, correct.Id)]);

        Assert.Equal(1, result.CorrectAnswerCount);
        Assert.Equal(1, result.TotalQuestionCount);
    }

    [Fact]
    public void Score_WithWrongAnswer_ReturnsZeroCorrect()
    {
        var (quiz, question, _, wrong) = CreateSingleQuestionQuiz();

        var result = quiz.Score([new QuizAnswerSubmission(question.Id, wrong.Id)]);

        Assert.Equal(0, result.CorrectAnswerCount);
    }

    [Fact]
    public void Score_WithMissingSubmission_ThrowsDomainException()
    {
        var (quiz, _, _, _) = CreateSingleQuestionQuiz();

        Assert.Throws<DomainException>(() => quiz.Score([]));
    }

    [Fact]
    public void Score_IgnoresClientProvidedScore_AndComputesFromSubmissions()
    {
        // Invariant 5: the domain always recomputes the score from submissions and the quiz
        // definition; there is no way to pass a pre-computed score into Score().
        var (quiz, question, _, wrong) = CreateSingleQuestionQuiz();

        var result = quiz.Score([new QuizAnswerSubmission(question.Id, wrong.Id)]);

        Assert.NotEqual(question.AnswerChoices.Count, result.CorrectAnswerCount + 999);
        Assert.Equal(0, result.CorrectAnswerCount);
    }
}
