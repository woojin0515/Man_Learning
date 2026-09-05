using ManLearning.Domain.Quizzes;

namespace ManLearning.Domain.Courses;

/// <summary>
/// A short unit of instructional content within a course. See invariant 1 and 2 in
/// docs/architecture/domain-model.md.
/// </summary>
public sealed class Lesson
{
    public LessonId Id { get; }
    public string Title { get; }
    public int Position { get; }
    public Quiz? Quiz { get; }

    public Lesson(LessonId id, string title, int position, Quiz? quiz = null)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainException("Lesson title cannot be empty.");
        }

        if (position < 0)
        {
            throw new DomainException("Lesson position cannot be negative.");
        }

        Id = id;
        Title = title;
        Position = position;
        Quiz = quiz;
    }
}
