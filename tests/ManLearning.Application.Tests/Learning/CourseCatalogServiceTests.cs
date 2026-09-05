using ManLearning.Application.Common;
using ManLearning.Application.Learning;
using ManLearning.Application.Tests.TestDoubles;
using ManLearning.Domain.Courses;

namespace ManLearning.Application.Tests.Learning;

public class CourseCatalogServiceTests
{
    [Fact]
    public async Task GetCourseAsync_WithExistingCourse_ReturnsMappedDto()
    {
        var courseRepository = new InMemoryCourseRepository();
        var course = new Course(CourseId.New(), "AI Fundamentals");
        course.AddLesson(new Lesson(LessonId.New(), "What is AI?", position: 0));
        courseRepository.Add(course);

        var service = new CourseCatalogService(courseRepository);

        var dto = await service.GetCourseAsync(course.Id);

        Assert.Equal(course.Title, dto.Title);
        Assert.Single(dto.Lessons);
        Assert.Equal("What is AI?", dto.Lessons[0].Title);
    }

    [Fact]
    public async Task GetCourseAsync_WithUnknownCourse_ThrowsNotFoundException()
    {
        var service = new CourseCatalogService(new InMemoryCourseRepository());

        await Assert.ThrowsAsync<NotFoundException>(() => service.GetCourseAsync(CourseId.New()));
    }
}
