using ManLearning.Application.Abstractions;
using ManLearning.Domain.Learners;

namespace ManLearning.Application.Tests.TestDoubles;

internal sealed class InMemoryStreakRepository : IStreakRepository
{
    private readonly Dictionary<LearnerId, Streak> _streaks = new();

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
