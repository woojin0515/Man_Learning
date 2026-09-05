using ManLearning.Domain.Courses;
using ManLearning.Domain.Learners;

namespace ManLearning.Domain.Progress;

/// <summary>
/// A learner's completion state for a single lesson. See invariant 6 in
/// docs/architecture/domain-model.md: progress cannot move backward through a completion
/// state. The exact policy for when a lesson becomes "completed" (for example, requiring a
/// passing quiz score) is an Application-layer decision, deferred per the domain model's open
/// decisions.
/// </summary>
public sealed class LessonProgress
{
    public LearnerId LearnerId { get; }
    public LessonId LessonId { get; }
    public LessonCompletionState State { get; private set; }

    public LessonProgress(LearnerId learnerId, LessonId lessonId)
    {
        LearnerId = learnerId;
        LessonId = lessonId;
        State = LessonCompletionState.NotStarted;
    }

    public void Start() => AdvanceTo(LessonCompletionState.InProgress);

    public void Complete() => AdvanceTo(LessonCompletionState.Completed);

    private void AdvanceTo(LessonCompletionState nextState)
    {
        if (nextState < State)
        {
            throw new DomainException(
                $"Lesson progress cannot move backward from {State} to {nextState}.");
        }

        State = nextState;
    }
}
