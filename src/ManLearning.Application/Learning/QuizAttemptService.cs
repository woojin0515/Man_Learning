using ManLearning.Application.Abstractions;
using ManLearning.Application.Common;
using ManLearning.Application.Learning.Dtos;
using ManLearning.Domain.Progress;
using ManLearning.Domain.Quizzes;
using ManLearning.Domain.Xp;

namespace ManLearning.Application.Learning;

/// <summary>
/// Orchestrates the core vertical slice: scoring a quiz attempt, advancing lesson progress, and
/// awarding XP on first completion. Scoring always happens in the Domain (<see cref="Quiz.Score"/>);
/// this service never trusts a client-provided score.
/// </summary>
public sealed class QuizAttemptService(
    ICourseRepository courseRepository,
    ILessonProgressRepository lessonProgressRepository,
    IXpAwardRepository xpAwardRepository,
    IDateTimeProvider dateTimeProvider)
{
    public async Task<QuizAttemptResultDto> SubmitQuizAttemptAsync(
        SubmitQuizAttemptCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var lesson = await courseRepository.FindLessonAsync(command.LessonId, cancellationToken)
            ?? throw new NotFoundException($"Lesson {command.LessonId} was not found.");

        var quiz = lesson.Quiz
            ?? throw new InvalidOperationException($"Lesson {command.LessonId} does not have a quiz.");

        var submissions = command.Answers
            .Select(answer => new QuizAnswerSubmission(answer.QuestionId, answer.SelectedAnswerChoiceId))
            .ToList();

        var result = quiz.Score(submissions);

        var progress = await lessonProgressRepository.FindAsync(
                command.LearnerId, command.LessonId, cancellationToken)
            ?? new LessonProgress(command.LearnerId, command.LessonId);

        var wasAlreadyCompleted = progress.State == LessonCompletionState.Completed;
        var xpAwarded = 0;

        if (!wasAlreadyCompleted && LessonCompletionPolicy.IsPassingScore(result))
        {
            progress.Start();
            progress.Complete();

            var award = new XpAward(
                command.LearnerId,
                LessonCompletionPolicy.LessonCompletionXpAmount,
                $"Completed lesson {command.LessonId}",
                dateTimeProvider.UtcNow);

            await xpAwardRepository.AddAsync(award, cancellationToken);
            xpAwarded = LessonCompletionPolicy.LessonCompletionXpAmount;
        }

        await lessonProgressRepository.SaveAsync(progress, cancellationToken);

        return new QuizAttemptResultDto(
            result.CorrectAnswerCount,
            result.TotalQuestionCount,
            progress.State == LessonCompletionState.Completed,
            xpAwarded);
    }
}
