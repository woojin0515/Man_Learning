namespace ManLearning.Domain.Courses;

/// <summary>
/// An ordered learning path made up of lessons. See invariant 1 and 2 in
/// docs/architecture/domain-model.md.
/// </summary>
public sealed class Course
{
    private readonly List<Lesson> _lessons = [];

    public CourseId Id { get; }
    public string Title { get; }
    public IReadOnlyList<Lesson> Lessons => _lessons;

    public Course(CourseId id, string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainException("Course title cannot be empty.");
        }

        Id = id;
        Title = title;
    }

    public void AddLesson(Lesson lesson)
    {
        ArgumentNullException.ThrowIfNull(lesson);

        if (_lessons.Any(existing => existing.Position == lesson.Position))
        {
            throw new DomainException(
                $"Course '{Title}' already contains a lesson at position {lesson.Position}.");
        }

        _lessons.Add(lesson);
    }
}
