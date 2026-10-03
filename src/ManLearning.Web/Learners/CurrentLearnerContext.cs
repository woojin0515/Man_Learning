using ManLearning.Domain.Learners;

namespace ManLearning.Web.Learners;

/// <summary>
/// Stands in for authenticated learner identity until the authentication model is designed (see
/// "Open decisions" in docs/architecture/domain-model.md). Registered as scoped so every
/// learner's Blazor Server circuit gets one stable, opaque identifier for the lifetime of their
/// session. This must be replaced once real sign-in exists; it is not a security boundary.
/// </summary>
public sealed class CurrentLearnerContext
{
    public LearnerId LearnerId { get; } = LearnerId.New();
}
