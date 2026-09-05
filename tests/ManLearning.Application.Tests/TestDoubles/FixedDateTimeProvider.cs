using ManLearning.Application.Abstractions;

namespace ManLearning.Application.Tests.TestDoubles;

internal sealed class FixedDateTimeProvider(DateTimeOffset utcNow) : IDateTimeProvider
{
    public DateTimeOffset UtcNow { get; } = utcNow;
}
