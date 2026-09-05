using ManLearning.Application.Common;
using ManLearning.Application.Learning;
using ManLearning.Application.Tests.TestDoubles;
using ManLearning.Domain.Courses;
using ManLearning.Domain.Learners;
using ManLearning.Domain.Progress;

namespace ManLearning.Application.Tests.Learning;

public class LessonProgressServiceTests
{
    [Fact]
    public async Task StartLessonAsync_WithExistingLesson_MarksProgressInProgress()
    {
        var courseRepository = new InMemoryCourseRepository();
        var course = new Course(CourseId.New(), "AI Fundamentals");
        var lesson = new Lesson(LessonId.New(), "What is AI?", position: 0);
        course.AddLesson(lesson);
        courseRepository.Add(course);

        var progressRepository = new InMemoryLessonProgressRepository();
        var service = new LessonProgressService(courseRepository, progressRepository);
        var learnerId = LearnerId.New();

        await service.StartLessonAsync(learnerId, lesson.Id);

        var progress = await progressRepository.FindAsync(learnerId, lesson.Id);
        Assert.NotNull(progress);
        Assert.Equal(LessonCompletionState.InProgress, progress!.State);
    }

    [Fact]
    public async Task StartLessonAsync_WithUnknownLesson_ThrowsNotFoundException()
    {
        var service = new LessonProgressService(
            new InMemoryCourseRepository(), new InMemoryLessonProgressRepository());

        await Assert.ThrowsAsync<NotFoundException>(
            () => service.StartLessonAsync(LearnerId.New(), LessonId.New()));
    }
}
