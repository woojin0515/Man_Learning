namespace ManLearning.Application.Learning.Dtos;

public sealed record QuizAttemptResultDto(
    int CorrectAnswerCount,
    int TotalQuestionCount,
    bool LessonCompleted,
    int XpAwarded);
