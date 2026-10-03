using ManLearning.Application.Abstractions;
using ManLearning.Domain.Courses;
using ManLearning.Domain.Learners;
using ManLearning.Domain.Progress;
using Microsoft.EntityFrameworkCore;

namespace ManLearning.Infrastructure.Persistence.EfCore;

/// <summary>
/// EF Core-backed <see cref="ILessonProgressRepository"/>. See ADR 0004
/// (docs/decisions/0004-database-technology.md).
/// </summary>
internal sealed class EfCoreLessonProgressRepository(ManLearningDbContext dbContext)
    : ILessonProgressRepository
{
    public async Task<LessonProgress?> FindAsync(
        LearnerId learnerId, LessonId lessonId, CancellationToken cancellationToken = default)
    {
        return await dbContext.LessonProgress
            .FirstOrDefaultAsync(
                progress => progress.LearnerId == learnerId && progress.LessonId == lessonId,
                cancellationToken);
    }

    public async Task<IReadOnlyList<LessonProgress>> GetAllForLearnerAsync(
        LearnerId learnerId, CancellationToken cancellationToken = default)
    {
        return await dbContext.LessonProgress
            .AsNoTracking()
            .Where(progress => progress.LearnerId == learnerId)
            .ToListAsync(cancellationToken);
    }

    public async Task SaveAsync(LessonProgress progress, CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.LessonProgress.FindAsync(
            [progress.LearnerId, progress.LessonId], cancellationToken);

        if (existing is null)
        {
            dbContext.LessonProgress.Add(progress);
        }
        else if (!ReferenceEquals(existing, progress))
        {
            dbContext.Entry(existing).CurrentValues.SetValues(progress);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
