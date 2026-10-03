using ManLearning.Domain.Xp;

namespace ManLearning.Domain.Tests.Xp;

public class LevelCurveTests
{
    [Theory]
    [InlineData(0, 1, 0, 100)]
    [InlineData(1, 1, 0, 100)]
    [InlineData(99, 1, 0, 100)]
    [InlineData(100, 2, 100, 200)]
    [InlineData(150, 2, 100, 200)]
    [InlineData(250, 3, 200, 300)]
    public void GetLevelProgress_ReturnsExpectedLevelAndThresholds(
        int totalXp, int expectedLevel, int expectedCurrentLevelXp, int expectedNextLevelXp)
    {
        var progress = LevelCurve.GetLevelProgress(totalXp);

        Assert.Equal(expectedLevel, progress.Level);
        Assert.Equal(expectedCurrentLevelXp, progress.CurrentLevelXp);
        Assert.Equal(expectedNextLevelXp, progress.NextLevelXp);
    }

    [Theory]
    [InlineData(0, 0.0)]
    [InlineData(50, 0.5)]
    [InlineData(99, 0.99)]
    [InlineData(100, 0.0)]
    [InlineData(150, 0.5)]
    public void GetLevelProgress_ReturnsExpectedProgressToNextLevel(int totalXp, double expectedProgress)
    {
        var progress = LevelCurve.GetLevelProgress(totalXp);

        Assert.Equal(expectedProgress, progress.ProgressToNextLevel, precision: 3);
    }

    [Fact]
    public void GetLevelProgress_WithNegativeXp_ThrowsDomainException()
    {
        Assert.Throws<DomainException>(() => LevelCurve.GetLevelProgress(-1));
    }
}
