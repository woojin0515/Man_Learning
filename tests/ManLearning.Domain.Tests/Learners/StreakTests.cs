using ManLearning.Domain.Learners;

namespace ManLearning.Domain.Tests.Learners;

public class StreakTests
{
    private static readonly LearnerId Learner = LearnerId.New();

    [Fact]
    public void RecordActivity_FirstEverActivity_StartsStreakAtOne()
    {
        var streak = new Streak(Learner);

        var changed = streak.RecordActivity(new DateOnly(2024, 1, 10));

        Assert.True(changed);
        Assert.Equal(1, streak.CurrentLength);
        Assert.Equal(1, streak.LongestLength);
        Assert.Equal(new DateOnly(2024, 1, 10), streak.LastActiveDate);
    }

    [Fact]
    public void RecordActivity_ConsecutiveDay_IncrementsStreak()
    {
        var streak = new Streak(Learner);
        streak.RecordActivity(new DateOnly(2024, 1, 10));

        var changed = streak.RecordActivity(new DateOnly(2024, 1, 11));

        Assert.True(changed);
        Assert.Equal(2, streak.CurrentLength);
        Assert.Equal(2, streak.LongestLength);
    }

    [Fact]
    public void RecordActivity_SameDayTwice_DoesNotChangeStreak()
    {
        var streak = new Streak(Learner);
        streak.RecordActivity(new DateOnly(2024, 1, 10));

        var changed = streak.RecordActivity(new DateOnly(2024, 1, 10));

        Assert.False(changed);
        Assert.Equal(1, streak.CurrentLength);
    }

    [Fact]
    public void RecordActivity_GapOfMoreThanOneDay_ResetsStreakToOne()
    {
        var streak = new Streak(Learner);
        streak.RecordActivity(new DateOnly(2024, 1, 10));
        streak.RecordActivity(new DateOnly(2024, 1, 11));

        var changed = streak.RecordActivity(new DateOnly(2024, 1, 20));

        Assert.True(changed);
        Assert.Equal(1, streak.CurrentLength);
        Assert.Equal(2, streak.LongestLength);
    }

    [Fact]
    public void RecordActivity_AfterReset_LongestStreakIsPreserved()
    {
        var streak = new Streak(Learner);
        streak.RecordActivity(new DateOnly(2024, 1, 1));
        streak.RecordActivity(new DateOnly(2024, 1, 2));
        streak.RecordActivity(new DateOnly(2024, 1, 3));
        streak.RecordActivity(new DateOnly(2024, 1, 10));

        Assert.Equal(1, streak.CurrentLength);
        Assert.Equal(3, streak.LongestLength);
    }

    [Fact]
    public void Constructor_NegativeCurrentLength_Throws()
    {
        Assert.Throws<DomainException>(() => new Streak(Learner, currentLength: -1, longestLength: 0, lastActiveDate: null));
    }

    [Fact]
    public void Constructor_LongestShorterThanCurrent_Throws()
    {
        Assert.Throws<DomainException>(() => new Streak(Learner, currentLength: 5, longestLength: 2, lastActiveDate: null));
    }
}
