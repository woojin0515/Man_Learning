using ManLearning.Domain;
using ManLearning.Domain.Courses;
using ManLearning.Domain.Learners;
using ManLearning.Domain.Progress;

namespace ManLearning.Domain.Tests.Progress;

public class LessonProgressTests
{
    [Fact]
    public void NewProgress_StartsAsNotStarted()
    {
        var progress = new LessonProgress(LearnerId.New(), LessonId.New());

        Assert.Equal(LessonCompletionState.NotStarted, progress.State);
    }

    [Fact]
    public void Start_ThenComplete_AdvancesForward()
    {
        var progress = new LessonProgress(LearnerId.New(), LessonId.New());

        progress.Start();
        progress.Complete();

        Assert.Equal(LessonCompletionState.Completed, progress.State);
    }

    [Fact]
    public void Complete_ThenStart_ThrowsDomainException()
    {
        var progress = new LessonProgress(LearnerId.New(), LessonId.New());
        progress.Start();
        progress.Complete();

        Assert.Throws<DomainException>(progress.Start);
    }
}
