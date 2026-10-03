using ManLearning.Domain.Courses;
using ManLearning.Domain.Learners;
using ManLearning.Domain.Progress;
using ManLearning.Domain.Xp;
using Microsoft.EntityFrameworkCore;

namespace ManLearning.Infrastructure.Persistence.EfCore;

/// <summary>
/// EF Core context for the Man Learning domain. See ADR 0004
/// (docs/decisions/0004-database-technology.md): production targets Azure SQL Database, while
/// local development and tests may point this same context at another relational provider (for
/// example SQLite) since the entity configuration is provider-agnostic.
/// </summary>
public sealed class ManLearningDbContext(DbContextOptions<ManLearningDbContext> options)
    : DbContext(options)
{
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<LessonProgress> LessonProgress => Set<LessonProgress>();
    public DbSet<XpAward> XpAwards => Set<XpAward>();
    public DbSet<Streak> Streaks => Set<Streak>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ManLearningDbContext).Assembly);
    }
}
