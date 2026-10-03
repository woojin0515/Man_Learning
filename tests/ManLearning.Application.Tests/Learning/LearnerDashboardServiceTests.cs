using ManLearning.Application.Learning;
using ManLearning.Application.Tests.TestDoubles;
using ManLearning.Domain.Courses;
using ManLearning.Domain.Learners;
using ManLearning.Domain.Progress;
using ManLearning.Domain.Xp;

namespace ManLearning.Application.Tests.Learning;

public class LearnerDashboardServiceTests
{
    [Fact]
    public async Task GetDashboardAsync_WithNoActivity_ReturnsZeroedSnapshot()
    {
        var courseRepository = new InMemoryCourseRepository();
        var course = new Course(CourseId.New(), "AI Fundamentals");
        course.AddLesson(new Lesson(LessonId.New(), "What is AI?", position: 0));
        courseRepository.Add(course);

        var service = new LearnerDashboardService(
            courseRepository, new InMemoryLessonProgressRepository(), new InMemoryXpAwardRepository());

        var dashboard = await service.GetDashboardAsync(LearnerId.New());

        Assert.Equal(0, dashboard.TotalXp);
        Assert.Equal(0, dashboard.CompletedLessonCount);
        Assert.Equal(1, dashboard.TotalLessonCount);
        Assert.Equal(0, dashboard.CourseProgress.Single().CompletedLessonCount);
    }

    [Fact]
    public async Task GetDashboardAsync_SumsXpAndCompletedLessonsAcrossCourses()
    {
        var courseRepository = new InMemoryCourseRepository();

        var course1 = new Course(CourseId.New(), "AI Fundamentals");
        var lesson1 = new Lesson(LessonId.New(), "What is AI?", position: 0);
        var lesson2 = new Lesson(LessonId.New(), "History of AI", position: 1);
        course1.AddLesson(lesson1);
        course1.AddLesson(lesson2);
        courseRepository.Add(course1);

        var course2 = new Course(CourseId.New(), "Machine Learning");
        var lesson3 = new Lesson(LessonId.New(), "What is ML?", position: 0);
        course2.AddLesson(lesson3);
        courseRepository.Add(course2);

        var learnerId = LearnerId.New();

        var progressRepository = new InMemoryLessonProgressRepository();
        var completedProgress = new LessonProgress(learnerId, lesson1.Id);
        completedProgress.Start();
        completedProgress.Complete();
        await progressRepository.SaveAsync(completedProgress);

        var inProgressProgress = new LessonProgress(learnerId, lesson2.Id);
        inProgressProgress.Start();
        await progressRepository.SaveAsync(inProgressProgress);

        var xpAwardRepository = new InMemoryXpAwardRepository();
        await xpAwardRepository.AddAsync(
            new XpAward(learnerId, 10, "Completed lesson", DateTimeOffset.UtcNow));
        await xpAwardRepository.AddAsync(
            new XpAward(learnerId, 10, "Completed another lesson", DateTimeOffset.UtcNow));

        // XP and progress from a different learner must not leak into this learner's dashboard.
        var otherLearnerId = LearnerId.New();
        await xpAwardRepository.AddAsync(
            new XpAward(otherLearnerId, 999, "Someone else's award", DateTimeOffset.UtcNow));

        var service = new LearnerDashboardService(courseRepository, progressRepository, xpAwardRepository);

        var dashboard = await service.GetDashboardAsync(learnerId);

        Assert.Equal(20, dashboard.TotalXp);
        Assert.Equal(1, dashboard.CompletedLessonCount);
        Assert.Equal(3, dashboard.TotalLessonCount);
        Assert.Equal(2, dashboard.CourseProgress.Count);

        var course1Progress = dashboard.CourseProgress.Single(c => c.CourseId == course1.Id);
        Assert.Equal(1, course1Progress.CompletedLessonCount);
        Assert.Equal(2, course1Progress.TotalLessonCount);

        var course2Progress = dashboard.CourseProgress.Single(c => c.CourseId == course2.Id);
        Assert.Equal(0, course2Progress.CompletedLessonCount);
        Assert.Equal(1, course2Progress.TotalLessonCount);
    }
}
