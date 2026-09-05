using ManLearning.Domain.Xp;

namespace ManLearning.Application.Abstractions;

/// <summary>
/// Persistence abstraction for recorded XP awards.
/// </summary>
public interface IXpAwardRepository
{
    Task AddAsync(XpAward award, CancellationToken cancellationToken = default);
}
