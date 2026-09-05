using ManLearning.Domain;
using ManLearning.Domain.Courses;

namespace ManLearning.Domain.Tests.Courses;

public class LessonTests
{
    [Fact]
    public void Constructor_WithEmptyTitle_ThrowsDomainException()
    {
        Assert.Throws<DomainException>(() => new Lesson(LessonId.New(), "", position: 0));
    }

    [Fact]
    public void Constructor_WithNegativePosition_ThrowsDomainException()
    {
        Assert.Throws<DomainException>(() => new Lesson(LessonId.New(), "Intro", position: -1));
    }
}
