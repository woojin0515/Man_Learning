using ManLearning.Domain.Courses;
using ManLearning.Domain.Learners;

namespace ManLearning.Application.Learning.Dtos;

public sealed record SubmitQuizAttemptCommand(
    LearnerId LearnerId,
    LessonId LessonId,
    IReadOnlyList<QuestionAnswerDto> Answers);
