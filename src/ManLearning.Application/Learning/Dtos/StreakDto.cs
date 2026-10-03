namespace ManLearning.Application.Learning.Dtos;

/// <summary>
/// Read model for a learner's streak, projected from <see cref="ManLearning.Domain.Learners.Streak"/>
/// so the Web layer never depends on Domain types directly.
/// </summary>
public sealed record StreakDto(int CurrentLength, int LongestLength, DateOnly? LastActiveDate);
