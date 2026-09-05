using ManLearning.Application.Abstractions;
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
}
