using System.Collections.Concurrent;
using ManLearning.Application.Abstractions;
using ManLearning.Domain.Learners;

namespace ManLearning.Infrastructure.Persistence;

/// <summary>
/// In-memory <see cref="IStreakRepository"/>. Streak state is lost on process restart; this is a
/// temporary stand-in until the database technology spike is completed.
/// </summary>
public sealed class InMemoryStreakRepository : IStreakRepository
{
    private readonly ConcurrentDictionary<LearnerId, Streak> _streaks = new();

    public Task<Streak?> FindAsync(LearnerId learnerId, CancellationToken cancellationToken = default)
    {
        _streaks.TryGetValue(learnerId, out var streak);
        return Task.FromResult(streak);
    }

    public Task SaveAsync(Streak streak, CancellationToken cancellationToken = default)
    {
        _streaks[streak.LearnerId] = streak;
        return Task.CompletedTask;
    }
}
