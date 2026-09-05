namespace ManLearning.Domain.Quizzes;

/// <summary>
/// The outcome of scoring a quiz attempt. Contains only the raw result; pass/fail policy and
/// completion decisions belong to the Application layer (see "Open decisions for later work"
/// in docs/architecture/domain-model.md).
/// </summary>
public sealed class QuizAttemptResult
{
    public QuizId QuizId { get; }
    public int CorrectAnswerCount { get; }
    public int TotalQuestionCount { get; }

    public QuizAttemptResult(QuizId quizId, int correctAnswerCount, int totalQuestionCount)
    {
        if (totalQuestionCount <= 0)
        {
            throw new DomainException("A quiz attempt result must reference at least one question.");
        }

        if (correctAnswerCount < 0 || correctAnswerCount > totalQuestionCount)
        {
            throw new DomainException(
                "Correct answer count must be between zero and the total question count.");
        }

        QuizId = quizId;
        CorrectAnswerCount = correctAnswerCount;
        TotalQuestionCount = totalQuestionCount;
    }
}
