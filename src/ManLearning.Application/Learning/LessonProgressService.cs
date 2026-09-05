using ManLearning.Application.Abstractions;
using ManLearning.Application.Common;
using ManLearning.Domain.Courses;
using ManLearning.Domain.Learners;
using ManLearning.Domain.Progress;

namespace ManLearning.Application.Learning;

/// <summary>
/// Use case for tracking a learner's progress through a lesson that does not require quiz
/// scoring to advance (see <see cref="QuizAttemptService"/> for quiz-gated completion).
/// </summary>
public sealed class LessonProgressService(
    ICourseRepository courseRepository,
    ILessonProgressRepository lessonProgressRepository)
{
    public async Task StartLessonAsync(
        LearnerId learnerId, LessonId lessonId, CancellationToken cancellationToken = default)
    {
        _ = await courseRepository.FindLessonAsync(lessonId, cancellationToken)
            ?? throw new NotFoundException($"Lesson {lessonId} was not found.");

        var progress = await lessonProgressRepository.FindAsync(learnerId, lessonId, cancellationToken)
            ?? new LessonProgress(learnerId, lessonId);

        progress.Start();

        await lessonProgressRepository.SaveAsync(progress, cancellationToken);
    }
}
