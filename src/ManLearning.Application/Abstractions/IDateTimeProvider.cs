namespace ManLearning.Application.Abstractions;

/// <summary>
/// An application-level abstraction over the system clock so use cases stay testable and do not
/// call DateTimeOffset.UtcNow directly.
/// </summary>
public interface IDateTimeProvider
{
    DateTimeOffset UtcNow { get; }
}
