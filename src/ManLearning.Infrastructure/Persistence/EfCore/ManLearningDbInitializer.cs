using Microsoft.EntityFrameworkCore;

namespace ManLearning.Infrastructure.Persistence.EfCore;

/// <summary>
/// Prepares the database schema and ensures the demo course catalog exists on startup.
/// </summary>
public static class ManLearningDbInitializer
{
    /// <summary>
    /// Creates/updates the schema and seeds demo data. The versioned migrations under
    /// <c>Persistence/EfCore/Migrations</c> are authored against the SQL Server provider (the
    /// ADR 0004 production target), so they are only applied when the context is actually using
    /// SQL Server. The local SQLite fallback (see
    /// <see cref="InfrastructureServiceCollectionExtensions.AddManLearningInfrastructure"/>) is a
    /// developer convenience, not a schema-of-record, so it uses
    /// <see cref="RelationalDatabaseFacadeExtensions.EnsureCreatedAsync"/> to build a schema
    /// straight from the current model instead of sharing SQL Server's migration history (the two
    /// providers render column types differently, so one migration set cannot serve both).
    /// </summary>
    public static async Task InitializeAsync(
        ManLearningDbContext dbContext, CancellationToken cancellationToken = default)
    {
        if (dbContext.Database.IsSqlServer())
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
        }
        else
        {
            await dbContext.Database.EnsureCreatedAsync(cancellationToken);
        }

        await SeedDemoCourseCatalogAsync(dbContext, cancellationToken);
    }

    private static async Task SeedDemoCourseCatalogAsync(
        ManLearningDbContext dbContext, CancellationToken cancellationToken)
    {
        if (await dbContext.Courses.AnyAsync(cancellationToken))
        {
            return;
        }

        dbContext.Courses.AddRange(SeedCourseData.BuildCourses());
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
