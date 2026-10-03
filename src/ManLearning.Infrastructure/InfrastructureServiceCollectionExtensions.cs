using ManLearning.Application.Abstractions;
using ManLearning.Infrastructure.Persistence.EfCore;
using ManLearning.Infrastructure.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ManLearning.Infrastructure;

/// <summary>
/// Composition root helper for registering Infrastructure implementations against the
/// Application-layer abstractions. Callers (the Web host) depend only on this method, not on the
/// concrete types inside Infrastructure.
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{
    /// <summary>
    /// Registers Infrastructure implementations, including the EF Core <see cref="ManLearningDbContext"/>.
    /// See ADR 0004 (docs/decisions/0004-database-technology.md): production reads the
    /// "ManLearningDb" connection string (configured through Azure App Service Configuration) and
    /// targets Azure SQL Database. When that connection string is not configured — the common
    /// case for local development without a provisioned database — this falls back to a local
    /// SQLite file so `dotnet run` keeps working without extra setup.
    /// </summary>
    public static IServiceCollection AddManLearningInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ManLearningDb");

        services.AddDbContext<ManLearningDbContext>(options =>
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                options.UseSqlite("Data Source=manlearning.dev.db");
            }
            else
            {
                options.UseSqlServer(connectionString);
            }
        });

        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        services.AddScoped<ICourseRepository, EfCoreCourseRepository>();
        services.AddScoped<ILessonProgressRepository, EfCoreLessonProgressRepository>();
        services.AddScoped<IXpAwardRepository, EfCoreXpAwardRepository>();
        services.AddScoped<IStreakRepository, EfCoreStreakRepository>();

        return services;
    }
}
