namespace ManLearning.Application.Common;

/// <summary>
/// Thrown when a requested aggregate could not be found. Kept as a single shared type so that
/// callers (for example the Web project) can map it to a consistent "not found" response.
/// </summary>
public sealed class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}
