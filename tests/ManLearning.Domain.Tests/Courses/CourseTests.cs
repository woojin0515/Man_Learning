using ManLearning.Domain;
using ManLearning.Domain.Courses;

namespace ManLearning.Domain.Tests.Courses;

public class CourseTests
{
    [Fact]
    public void Constructor_WithEmptyTitle_ThrowsDomainException()
    {
        Assert.Throws<DomainException>(() => new Course(CourseId.New(), "   "));
    }

    [Fact]
    public void AddLesson_WithUniquePositions_AddsAllLessons()
    {
        var course = new Course(CourseId.New(), "AI Fundamentals");

        course.AddLesson(new Lesson(LessonId.New(), "What is AI?", position: 0));
        course.AddLesson(new Lesson(LessonId.New(), "History of AI", position: 1));

        Assert.Equal(2, course.Lessons.Count);
    }

    [Fact]
    public void AddLesson_WithDuplicatePosition_ThrowsDomainException()
    {
        var course = new Course(CourseId.New(), "AI Fundamentals");
        course.AddLesson(new Lesson(LessonId.New(), "What is AI?", position: 0));

        Assert.Throws<DomainException>(
            () => course.AddLesson(new Lesson(LessonId.New(), "Duplicate", position: 0)));
    }
}
