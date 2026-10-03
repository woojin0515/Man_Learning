using ManLearning.Application.Abstractions;
using ManLearning.Domain.Courses;
using Microsoft.EntityFrameworkCore;

namespace ManLearning.Infrastructure.Persistence.EfCore;

/// <summary>
/// EF Core-backed <see cref="ICourseRepository"/>. See ADR 0004
/// (docs/decisions/0004-database-technology.md).
/// </summary>
internal sealed class EfCoreCourseRepository(ManLearningDbContext dbContext) : ICourseRepository
{
    public async Task<IReadOnlyList<Course>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Courses
            .AsNoTracking()
            .Include(course => course.Lessons)
            .ThenInclude(lesson => lesson.Quiz!.Questions)
            .ThenInclude(question => question.AnswerChoices)
            .ToListAsync(cancellationToken);
    }

    public async Task<Course?> GetByIdAsync(CourseId courseId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Courses
            .AsNoTracking()
            .Include(course => course.Lessons)
            .ThenInclude(lesson => lesson.Quiz!.Questions)
            .ThenInclude(question => question.AnswerChoices)
            .FirstOrDefaultAsync(course => course.Id == courseId, cancellationToken);
    }

    public async Task<Lesson?> FindLessonAsync(LessonId lessonId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<Lesson>()
            .AsNoTracking()
            .Include(lesson => lesson.Quiz!.Questions)
            .ThenInclude(question => question.AnswerChoices)
            .FirstOrDefaultAsync(lesson => lesson.Id == lessonId, cancellationToken);
    }
}
