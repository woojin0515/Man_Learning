using ManLearning.Domain.Courses;
using ManLearning.Domain.Quizzes;

namespace ManLearning.Infrastructure.Persistence;

/// <summary>
/// Hardcoded demo course content for the first web vertical slice. This stands in for a real
/// content store until the database technology spike (see docs/architecture/domain-model.md)
/// is completed; it must not be treated as a long-term content authoring approach.
/// </summary>
internal static class SeedCourseData
{
    public static IReadOnlyList<Course> BuildCourses()
    {
        var course = new Course(
            new CourseId(Guid.Parse("8f1a1b1e-1111-4a1a-9a1a-000000000001")),
            "AI 기초 첫걸음");

        course.AddLesson(new Lesson(
            new LessonId(Guid.Parse("8f1a1b1e-2222-4a1a-9a1a-000000000001")),
            "인공지능이란 무엇인가",
            position: 0));

        course.AddLesson(new Lesson(
            new LessonId(Guid.Parse("8f1a1b1e-2222-4a1a-9a1a-000000000002")),
            "AI의 종류 구분하기",
            position: 1,
            quiz: BuildAiTypesQuiz()));

        return [course];
    }

    private static Quiz BuildAiTypesQuiz()
    {
        var question1 = new Question(
            new QuestionId(Guid.Parse("8f1a1b1e-3333-4a1a-9a1a-000000000001")),
            "사람처럼 모든 지적 작업을 수행할 수 있는, 아직 실현되지 않은 AI를 부르는 이름은?",
            [
                new AnswerChoice(
                    new AnswerChoiceId(Guid.Parse("8f1a1b1e-4444-4a1a-9a1a-000000000001")),
                    "약한 인공지능 (Narrow AI)",
                    isCorrect: false),
                new AnswerChoice(
                    new AnswerChoiceId(Guid.Parse("8f1a1b1e-4444-4a1a-9a1a-000000000002")),
                    "강한 인공지능 (General AI)",
                    isCorrect: true),
                new AnswerChoice(
                    new AnswerChoiceId(Guid.Parse("8f1a1b1e-4444-4a1a-9a1a-000000000003")),
                    "규칙 기반 시스템 (Rule-based System)",
                    isCorrect: false),
            ]);

        var question2 = new Question(
            new QuestionId(Guid.Parse("8f1a1b1e-3333-4a1a-9a1a-000000000002")),
            "이메일 스팸 필터, 추천 시스템처럼 특정 작업만 잘 수행하는 AI를 부르는 이름은?",
            [
                new AnswerChoice(
                    new AnswerChoiceId(Guid.Parse("8f1a1b1e-4444-4a1a-9a1a-000000000004")),
                    "약한 인공지능 (Narrow AI)",
                    isCorrect: true),
                new AnswerChoice(
                    new AnswerChoiceId(Guid.Parse("8f1a1b1e-4444-4a1a-9a1a-000000000005")),
                    "초인공지능 (Superintelligence)",
                    isCorrect: false),
            ]);

        var question3 = new Question(
            new QuestionId(Guid.Parse("8f1a1b1e-3333-4a1a-9a1a-000000000003")),
            "오늘날 실제로 서비스되고 있는 AI는 대부분 어떤 유형인가?",
            [
                new AnswerChoice(
                    new AnswerChoiceId(Guid.Parse("8f1a1b1e-4444-4a1a-9a1a-000000000006")),
                    "강한 인공지능",
                    isCorrect: false),
                new AnswerChoice(
                    new AnswerChoiceId(Guid.Parse("8f1a1b1e-4444-4a1a-9a1a-000000000007")),
                    "약한 인공지능",
                    isCorrect: true),
            ]);

        return new Quiz(
            new QuizId(Guid.Parse("8f1a1b1e-5555-4a1a-9a1a-000000000001")),
            [question1, question2, question3]);
    }
}
