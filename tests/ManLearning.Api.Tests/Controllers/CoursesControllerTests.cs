using System.Net;
using System.Net.Http.Json;
using ManLearning.Api.Contracts;

namespace ManLearning.Api.Tests.Controllers;

/// <summary>
/// End-to-end test for the first API vertical slice: a real HTTP request runs through
/// <c>CoursesController</c> → <c>CourseCatalogService</c> (Application) → the EF Core course
/// repository (Infrastructure) → a real (in-memory SQLite) database, verifying the full call
/// boundary ADR-0006 describes, not just an isolated controller method call.
/// </summary>
public sealed class CoursesControllerTests(ManLearningApiFactory factory)
    : IClassFixture<ManLearningApiFactory>
{
    [Fact]
    public async Task GetCourses_ReturnsOkWithSeededCourseCatalog()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync("/api/courses");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var courses = await response.Content.ReadFromJsonAsync<List<CourseResponse>>();

        Assert.NotNull(courses);
        // ManLearningDbInitializer seeds exactly one demo course on startup (see
        // SeedCourseData); this assertion is intentionally tied to that known seed data rather
        // than merely checking the collection is non-null, so a regression that silently drops
        // the course data is actually caught.
        var course = Assert.Single(courses);
        Assert.NotEqual(Guid.Empty, course.Id);
        Assert.False(string.IsNullOrWhiteSpace(course.Title));
        Assert.True(course.LessonCount > 0);
    }

    [Fact]
    public async Task GetCourse_WithExistingCourse_ReturnsOkWithOrderedLessonSummaries()
    {
        var client = factory.CreateClient();

        // Known seed id from SeedCourseData: the "AI 기초 첫걸음" demo course.
        var response = await client.GetAsync("/api/courses/8f1a1b1e-1111-4a1a-9a1a-000000000001");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var course = await response.Content.ReadFromJsonAsync<CourseDetailResponse>();

        Assert.NotNull(course);
        Assert.Equal(Guid.Parse("8f1a1b1e-1111-4a1a-9a1a-000000000001"), course.Id);
        Assert.False(string.IsNullOrWhiteSpace(course.Title));
        Assert.Equal(2, course.Lessons.Count);
        // SeedCourseData adds lessons at positions 0 and 1, and the first has no quiz while the
        // second does — assert both the ordering and the quiz-presence flag the UI depends on.
        Assert.Equal(0, course.Lessons[0].Position);
        Assert.False(course.Lessons[0].HasQuiz);
        Assert.Equal(1, course.Lessons[1].Position);
        Assert.True(course.Lessons[1].HasQuiz);
    }

    [Fact]
    public async Task GetCourse_WithUnknownCourse_ReturnsNotFound()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync($"/api/courses/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
