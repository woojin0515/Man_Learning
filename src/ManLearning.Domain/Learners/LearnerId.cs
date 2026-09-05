namespace ManLearning.Domain.Learners;

/// <summary>
/// An opaque identifier for a learner. The identity and authentication model is an open
/// decision (see docs/architecture/domain-model.md); the domain only needs a stable identifier.
/// </summary>
public readonly record struct LearnerId(Guid Value)
{
    public static LearnerId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();
}
