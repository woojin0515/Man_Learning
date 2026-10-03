using System.Collections.Concurrent;
using ManLearning.Application.Abstractions;
using ManLearning.Domain.Courses;
using ManLearning.Domain.Learners;
using ManLearning.Domain.Progress;

namespace ManLearning.Infrastructure.Persistence;

/// <summary>
/// In-memory <see cref="ILessonProgressRepository"/>. Progress is lost on process restart; this
/// is a temporary stand-in until the database technology spike is completed.
/// </summary>
public sealed class InMemoryLessonProgressRepository : ILessonProgressRepository
{
    private readonly ConcurrentDictionary<(LearnerId LearnerId, LessonId LessonId), LessonProgress> _progressByKey = new();

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
