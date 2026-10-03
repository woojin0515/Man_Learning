using ManLearning.Domain.Xp;

using ManLearning.Domain.Learners;

namespace ManLearning.Application.Abstractions;

/// <summary>
/// Persistence abstraction for recorded XP awards.
/// </summary>
public interface IXpAwardRepository
{
    Task AddAsync(XpAward award, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns every XP award recorded for a learner, used to compute total XP for dashboard
    /// views. Summing happens in Application, not here, to keep this abstraction simple.
    /// </summary>
    Task<IReadOnlyList<XpAward>> GetByLearnerAsync(
        LearnerId learnerId, CancellationToken cancellationToken = default);
}
