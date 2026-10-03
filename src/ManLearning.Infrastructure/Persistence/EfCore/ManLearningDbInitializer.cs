using Microsoft.EntityFrameworkCore;

namespace ManLearning.Infrastructure.Persistence.EfCore;

/// <summary>
/// Ensures the demo course catalog exists on startup. This stands in for a real content
/// authoring/import pipeline until one is designed (see <see cref="SeedCourseData"/>); it must
/// not be treated as a long-term content authoring approach.
/// </summary>
public static class ManLearningDbInitializer
{
    public static async Task SeedDemoCourseCatalogAsync(
        ManLearningDbContext dbContext, CancellationToken cancellationToken = default)
    {
        if (await dbContext.Courses.AnyAsync(cancellationToken))
        {
            return;
        }

        dbContext.Courses.AddRange(SeedCourseData.BuildCourses());
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
