namespace ManLearning.Domain.Learners;

/// <summary>
/// Tracks a learner's consecutive learning days. See invariant 8 in
/// docs/architecture/domain-model.md ("a streak advances at most once per calendar day for a
/// learner") and docs/decisions/0003-initial-streak-calendar-policy.md for the calendar rule
/// this type implements. The caller is responsible for deriving <paramref name="today"/> from
/// UTC (via the application's date/time abstraction); this type has no notion of time zones.
/// </summary>
public sealed class Streak
{
    public LearnerId LearnerId { get; }
    public int CurrentLength { get; private set; }
    public int LongestLength { get; private set; }
    public DateOnly? LastActiveDate { get; private set; }

    public Streak(LearnerId learnerId)
        : this(learnerId, currentLength: 0, longestLength: 0, lastActiveDate: null)
    {
    }

    public Streak(LearnerId learnerId, int currentLength, int longestLength, DateOnly? lastActiveDate)
    {
        if (currentLength < 0)
        {
            throw new DomainException("Streak length cannot be negative.");
        }

        if (longestLength < currentLength)
        {
            throw new DomainException("Longest streak length cannot be shorter than the current streak.");
        }

        LearnerId = learnerId;
        CurrentLength = currentLength;
        LongestLength = longestLength;
        LastActiveDate = lastActiveDate;
    }

    /// <summary>
    /// Records a learning activity for the given UTC calendar day. Returns <c>true</c> when the
    /// streak length changed as a result (i.e. this was the first activity recorded today).
    /// </summary>
    public bool RecordActivity(DateOnly today)
    {
        if (LastActiveDate == today)
        {
            return false;
        }

        CurrentLength = LastActiveDate == today.AddDays(-1)
            ? CurrentLength + 1
            : 1;

        LastActiveDate = today;
        LongestLength = Math.Max(LongestLength, CurrentLength);

        return true;
    }
}
