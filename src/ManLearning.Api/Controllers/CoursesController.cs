using ManLearning.Api.Contracts;
using ManLearning.Application.Common;
using ManLearning.Application.Learning;
using ManLearning.Domain.Courses;
using Microsoft.AspNetCore.Mvc;

namespace ManLearning.Api.Controllers;

/// <summary>
/// HTTP boundary for browsing the course catalog. Per ADR-0006, this controller only adapts HTTP
/// requests/responses to <see cref="CourseCatalogService"/> (Application); it must never query
/// <see cref="ManLearning.Infrastructure.Persistence.EfCore.ManLearningDbContext"/> or any other
/// Infrastructure/Domain type directly.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class CoursesController(CourseCatalogService courseCatalogService) : ControllerBase
{
    /// <summary>
    /// Returns every course in the catalog. First vertical slice: no pagination, no filtering, no
    /// authentication (see ADR-0007 for the authentication work deferred from this endpoint).
    /// </summary>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<CourseResponse>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<CourseResponse>>> GetCourses(
        CancellationToken cancellationToken)
    {
        var courses = await courseCatalogService.GetCourseListAsync(cancellationToken);

        return Ok(courses.Select(CourseResponse.FromDto).ToList());
    }

    /// <summary>
    /// Returns a single course and its ordered lesson summaries, for the Course Detail / Lesson
    /// List screen. Anonymous-accessible, same as <see cref="GetCourses"/> (see ADR-0007).
    /// </summary>
    [HttpGet("{courseId:guid}")]
    [ProducesResponseType<CourseDetailResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CourseDetailResponse>> GetCourse(
        Guid courseId, CancellationToken cancellationToken)
    {
        try
        {
            var course = await courseCatalogService.GetCourseAsync(new CourseId(courseId), cancellationToken);
            return Ok(CourseDetailResponse.FromDto(course));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }
}

