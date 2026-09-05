using ManLearning.Domain.Learners;

namespace ManLearning.Domain.Xp;

/// <summary>
/// A record of experience earned from a completed learning action. See invariant 7 in
/// docs/architecture/domain-model.md: XP is awarded through explicit domain actions and cannot
/// be negative. The XP curve and level thresholds are deferred open decisions.
/// </summary>
public sealed class XpAward
{
    public LearnerId LearnerId { get; }
    public int Amount { get; }
    public string Reason { get; }
    public DateTimeOffset AwardedAtUtc { get; }

    public XpAward(LearnerId learnerId, int amount, string reason, DateTimeOffset awardedAtUtc)
    {
        if (amount < 0)
        {
            throw new DomainException("XP amount cannot be negative.");
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new DomainException("XP award reason cannot be empty.");
        }

        LearnerId = learnerId;
        Amount = amount;
        Reason = reason;
        AwardedAtUtc = awardedAtUtc;
    }
}
