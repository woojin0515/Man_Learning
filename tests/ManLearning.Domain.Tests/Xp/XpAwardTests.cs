using ManLearning.Domain;
using ManLearning.Domain.Learners;
using ManLearning.Domain.Xp;

namespace ManLearning.Domain.Tests.Xp;

public class XpAwardTests
{
    [Fact]
    public void Constructor_WithNegativeAmount_ThrowsDomainException()
    {
        Assert.Throws<DomainException>(
            () => new XpAward(LearnerId.New(), -1, "Lesson completed", DateTimeOffset.UtcNow));
    }

    [Fact]
    public void Constructor_WithEmptyReason_ThrowsDomainException()
    {
        Assert.Throws<DomainException>(
            () => new XpAward(LearnerId.New(), 10, "", DateTimeOffset.UtcNow));
    }

    [Fact]
    public void Constructor_WithValidArguments_CreatesAward()
    {
        var awardedAt = DateTimeOffset.UtcNow;
        var award = new XpAward(LearnerId.New(), 10, "Lesson completed", awardedAt);

        Assert.Equal(10, award.Amount);
        Assert.Equal(awardedAt, award.AwardedAtUtc);
    }
}
