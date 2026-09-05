using ManLearning.Application.Abstractions;
using ManLearning.Domain.Courses;
using ManLearning.Domain.Learners;
using ManLearning.Domain.Progress;

namespace ManLearning.Application.Tests.TestDoubles;

internal sealed class InMemoryLessonProgressRepository : ILessonProgressRepository
{
    private readonly Dictionary<(LearnerId, LessonId), LessonProgress> _progressByKey = [];

    public Task<LessonProgress?> FindAsync(
        LearnerId learnerId, LessonId lessonId, CancellationToken cancellationToken = default)
    {
        _progressByKey.TryGetValue((learnerId, lessonId), out var progress);
        return Task.FromResult(progress);
    }

    public Task SaveAsync(LessonProgress progress, CancellationToken cancellationToken = default)
    {
        _progressByKey[(progress.LearnerId, progress.LessonId)] = progress;
        return Task.CompletedTask;
    }
}
