import 'package:man_learning_api_client/man_learning_api_client.dart';

/// App-level representation of a single answer choice belonging to a [Question]. Never exposes
/// which choice is correct — `AnswerChoiceResponse` (and the Domain/Application layers beneath
/// it) already withholds that information from this read-only lesson-metadata API, and this
/// model preserves that boundary rather than reintroducing it.
class AnswerChoice {
  const AnswerChoice({required this.id, required this.text});

  final String id;
  final String text;

  factory AnswerChoice.fromResponse(AnswerChoiceResponse response) =>
      AnswerChoice(id: response.id, text: response.text);
}

/// App-level representation of a single quiz question and its answer choices.
class Question {
  const Question({required this.id, required this.text, required this.answerChoices});

  final String id;
  final String text;
  final List<AnswerChoice> answerChoices;

  factory Question.fromResponse(QuestionResponse response) => Question(
    id: response.id,
    text: response.text,
    answerChoices: response.answerChoices.map(AnswerChoice.fromResponse).toList(growable: false),
  );
}

/// App-level representation of a lesson's quiz. This vertical slice only reads quiz *metadata*
/// (questions and answer choice text) for display — answering/submitting a quiz is explicitly out
/// of scope (see the Lesson Vertical Slice task's EXCLUDE list).
class Quiz {
  const Quiz({required this.id, required this.questions});

  final String id;
  final List<Question> questions;

  factory Quiz.fromResponse(QuizResponse response) => Quiz(
    id: response.id,
    questions: response.questions.map(Question.fromResponse).toList(growable: false),
  );
}

/// App-level representation of `GET /api/lessons/{lessonId}`.
///
/// Important: the Domain `Lesson` entity does not currently model lesson content/body (plain
/// text, Markdown, HTML, or structured blocks) — see the Lesson Vertical Slice read-only audit.
/// This model therefore intentionally has no `content`/`body` field. Modeling actual lesson
/// content is a separate future architectural decision, not something this app should fabricate.
class Lesson {
  const Lesson({
    required this.id,
    required this.title,
    required this.position,
    required this.quiz,
  });

  final String id;
  final String title;
  final int position;
  final Quiz? quiz;

  factory Lesson.fromResponse(LessonResponse response) => Lesson(
    id: response.id,
    title: response.title,
    position: response.position,
    quiz: response.quiz == null ? null : Quiz.fromResponse(response.quiz!),
  );
}
