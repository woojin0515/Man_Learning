using ManLearning.Application.Abstractions;
using ManLearning.Infrastructure.Persistence;
using ManLearning.Infrastructure.Time;
using Microsoft.Extensions.DependencyInjection;

namespace ManLearning.Infrastructure;

/// <summary>
/// Composition root helper for registering Infrastructure implementations against the
/// Application-layer abstractions. Callers (the Web host) depend only on this method, not on the
/// concrete types inside Infrastructure.
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddManLearningInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        services.AddSingleton<ICourseRepository, InMemoryCourseRepository>();
        services.AddSingleton<ILessonProgressRepository, InMemoryLessonProgressRepository>();
        services.AddSingleton<IXpAwardRepository, InMemoryXpAwardRepository>();

        return services;
    }
}
