using ManLearning.Api.Contracts;
using ManLearning.Application.Common;
using ManLearning.Application.Learning;
using ManLearning.Domain.Courses;
using Microsoft.AspNetCore.Mvc;

namespace ManLearning.Api.Controllers;

/// <summary>
/// HTTP boundary for browsing a single lesson's metadata. Per ADR-0006, this controller only
/// adapts HTTP requests/responses to <see cref="CourseCatalogService"/> (Application); it must
/// never query <see cref="ManLearning.Infrastructure.Persistence.EfCore.ManLearningDbContext"/>
/// or any other Infrastructure/Domain type directly.
/// </summary>
/// <remarks>
/// Lesson content/body is not currently modeled in the Domain (see
/// docs/architecture/domain-model.md, "Open decisions"); this endpoint exposes Lesson metadata
/// only (title, position, quiz structure), not lesson content. Quiz submission, progress, and XP
/// are separate, already-implemented Application use cases
/// (<see cref="QuizAttemptService"/>/<see cref="LessonProgressService"/>) that this vertical
/// slice intentionally does not expose yet.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
public sealed class LessonsController(CourseCatalogService courseCatalogService) : ControllerBase
{
    /// <summary>
    /// Returns a single lesson's metadata and quiz structure (if any), for the Lesson Detail
    /// screen. Anonymous-accessible, same as <see cref="CoursesController"/> (see ADR-0007).
    /// </summary>
    [HttpGet("{lessonId:guid}")]
    [ProducesResponseType<LessonResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LessonResponse>> GetLesson(
        Guid lessonId, CancellationToken cancellationToken)
    {
        try
        {
            var lesson = await courseCatalogService.GetLessonAsync(new LessonId(lessonId), cancellationToken);
            return Ok(LessonResponse.FromDto(lesson));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }
}
