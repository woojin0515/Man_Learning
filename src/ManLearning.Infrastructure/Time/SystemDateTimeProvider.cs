using ManLearning.Application.Abstractions;

namespace ManLearning.Infrastructure.Time;

/// <summary>
/// Production implementation of <see cref="IDateTimeProvider"/> backed by the system clock.
/// </summary>
public sealed class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
