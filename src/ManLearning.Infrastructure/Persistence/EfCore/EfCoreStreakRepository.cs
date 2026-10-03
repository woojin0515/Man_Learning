using ManLearning.Application.Abstractions;
using ManLearning.Domain.Learners;
using Microsoft.EntityFrameworkCore;

namespace ManLearning.Infrastructure.Persistence.EfCore;

/// <summary>
/// EF Core-backed <see cref="IStreakRepository"/>. See ADR 0004
/// (docs/decisions/0004-database-technology.md).
/// </summary>
internal sealed class EfCoreStreakRepository(ManLearningDbContext dbContext) : IStreakRepository
{
    public async Task<Streak?> FindAsync(LearnerId learnerId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Streaks
            .FirstOrDefaultAsync(streak => streak.LearnerId == learnerId, cancellationToken);
    }

    public async Task SaveAsync(Streak streak, CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.Streaks.FindAsync([streak.LearnerId], cancellationToken);

        if (existing is null)
        {
            dbContext.Streaks.Add(streak);
        }
        else if (!ReferenceEquals(existing, streak))
        {
            dbContext.Entry(existing).CurrentValues.SetValues(streak);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
