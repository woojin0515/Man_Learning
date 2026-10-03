using ManLearning.Application.Abstractions;
using ManLearning.Domain.Learners;
using ManLearning.Domain.Xp;

namespace ManLearning.Application.Tests.TestDoubles;

internal sealed class InMemoryXpAwardRepository : IXpAwardRepository
{
    public List<XpAward> Awards { get; } = [];

    public Task AddAsync(XpAward award, CancellationToken cancellationToken = default)
    {
        Awards.Add(award);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<XpAward>> GetByLearnerAsync(
        LearnerId learnerId, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<XpAward> result = [.. Awards.Where(award => award.LearnerId == learnerId)];
        return Task.FromResult(result);
    }
}
