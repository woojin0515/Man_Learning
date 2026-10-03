using ManLearning.Application.Abstractions;
using ManLearning.Domain.Learners;
using ManLearning.Domain.Xp;
using Microsoft.EntityFrameworkCore;

namespace ManLearning.Infrastructure.Persistence.EfCore;

/// <summary>
/// EF Core-backed <see cref="IXpAwardRepository"/>. See ADR 0004
/// (docs/decisions/0004-database-technology.md).
/// </summary>
internal sealed class EfCoreXpAwardRepository(ManLearningDbContext dbContext) : IXpAwardRepository
{
    public async Task AddAsync(XpAward award, CancellationToken cancellationToken = default)
    {
        dbContext.XpAwards.Add(award);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<XpAward>> GetByLearnerAsync(
        LearnerId learnerId, CancellationToken cancellationToken = default)
    {
        return await dbContext.XpAwards
            .AsNoTracking()
            .Where(award => award.LearnerId == learnerId)
            .ToListAsync(cancellationToken);
    }
}
