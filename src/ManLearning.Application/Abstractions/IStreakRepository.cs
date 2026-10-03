using ManLearning.Domain.Learners;

namespace ManLearning.Application.Abstractions;

/// <summary>
/// Persistence abstraction for a learner's streak state. There is at most one <see cref="Streak"/>
/// per learner, so this mirrors a simple upsert rather than an append-only log (unlike
/// <see cref="IXpAwardRepository"/>).
/// </summary>
public interface IStreakRepository
{
    Task<Streak?> FindAsync(LearnerId learnerId, CancellationToken cancellationToken = default);

    Task SaveAsync(Streak streak, CancellationToken cancellationToken = default);
}
