using ManLearning.Domain.Quizzes;

namespace ManLearning.Application.Learning;

/// <summary>
/// Provisional policy for the first vertical slice. A quiz-gated lesson completes only when
/// every question is answered correctly, and completing a lesson for the first time awards a
/// flat XP amount.
///
/// This is intentionally simple and is expected to change: the XP curve, difficulty-based
/// rewards, and partial-credit rules are open decisions (see "Open decisions for later work" in
/// docs/architecture/domain-model.md and docs/decisions/0001-initial-lesson-completion-and-xp-policy.md).
/// </summary>
public static class LessonCompletionPolicy
{
    public const int LessonCompletionXpAmount = 10;

    public static bool IsPassingScore(QuizAttemptResult result) =>
        result.CorrectAnswerCount == result.TotalQuestionCount;
}
