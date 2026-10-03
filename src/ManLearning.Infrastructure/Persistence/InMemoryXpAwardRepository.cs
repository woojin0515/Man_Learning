using System.Collections.Concurrent;
using ManLearning.Application.Abstractions;
using ManLearning.Domain.Learners;
using ManLearning.Domain.Xp;

namespace ManLearning.Infrastructure.Persistence;

/// <summary>
/// In-memory <see cref="IXpAwardRepository"/>. Awards are lost on process restart; this is a
/// temporary stand-in until the database technology spike is completed.
/// </summary>
public sealed class InMemoryXpAwardRepository : IXpAwardRepository
{
    private readonly ConcurrentBag<XpAward> _awards = [];

    public Task AddAsync(XpAward award, CancellationToken cancellationToken = default)
    {
        _awards.Add(award);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<XpAward>> GetByLearnerAsync(
        LearnerId learnerId, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<XpAward> result = [.. _awards.Where(award => award.LearnerId == learnerId)];
        return Task.FromResult(result);
    }
}
