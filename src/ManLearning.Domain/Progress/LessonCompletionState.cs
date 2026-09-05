namespace ManLearning.Domain.Progress;

/// <summary>
/// A learner's completion state for a lesson. Ordinal values are meaningful: progress may only
/// advance to a higher-valued state (see invariant 6 in docs/architecture/domain-model.md).
/// </summary>
public enum LessonCompletionState
{
    NotStarted = 0,
    InProgress = 1,
    Completed = 2
}
