namespace ManLearning.Domain;

/// <summary>
/// Thrown when a domain invariant defined in docs/architecture/domain-model.md is violated.
/// </summary>
public sealed class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
}
