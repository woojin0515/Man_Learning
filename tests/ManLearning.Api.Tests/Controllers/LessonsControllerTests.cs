using System.Net;
using System.Net.Http.Json;
using ManLearning.Api.Contracts;

namespace ManLearning.Api.Tests.Controllers;

/// <summary>
/// End-to-end test for the Lesson Detail vertical slice: a real HTTP request runs through
/// <c>LessonsController</c> → <c>CourseCatalogService</c> (Application) → the EF Core course
/// repository's <c>FindLessonAsync</c> (Infrastructure) → a real (in-memory SQLite) database.
/// </summary>
public sealed class LessonsControllerTests(ManLearningApiFactory factory)
    : IClassFixture<ManLearningApiFactory>
{
    [Fact]
    public async Task GetLesson_WithLessonWithoutQuiz_ReturnsOkWithNullQuiz()
    {
        var client = factory.CreateClient();

        // Known seed id from SeedCourseData: "인공지능이란 무엇인가", position 0, no quiz.
        var response = await client.GetAsync("/api/lessons/8f1a1b1e-2222-4a1a-9a1a-000000000001");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var lesson = await response.Content.ReadFromJsonAsync<LessonResponse>();

        Assert.NotNull(lesson);
        Assert.Equal(Guid.Parse("8f1a1b1e-2222-4a1a-9a1a-000000000001"), lesson.Id);
        Assert.False(string.IsNullOrWhiteSpace(lesson.Title));
        Assert.Equal(0, lesson.Position);
        Assert.Null(lesson.Quiz);
    }

    [Fact]
    public async Task GetLesson_WithLessonWithQuiz_ReturnsOkWithQuestionsAndAnswerChoices()
    {
        var client = factory.CreateClient();

        // Known seed id from SeedCourseData: "AI의 종류 구분하기", position 1, 3-question quiz.
        var response = await client.GetAsync("/api/lessons/8f1a1b1e-2222-4a1a-9a1a-000000000002");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var lesson = await response.Content.ReadFromJsonAsync<LessonResponse>();

        Assert.NotNull(lesson);
        Assert.Equal(1, lesson.Position);
        Assert.NotNull(lesson.Quiz);
        Assert.Equal(3, lesson.Quiz.Questions.Count);
        Assert.All(lesson.Quiz.Questions, question => Assert.NotEmpty(question.AnswerChoices));
    }

    [Fact]
    public async Task GetLesson_WithUnknownLesson_ReturnsNotFound()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync($"/api/lessons/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
