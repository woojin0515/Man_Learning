import 'package:man_learning_api_client/man_learning_api_client.dart';

/// App-level representation of a single lesson's metadata as listed inside a course (i.e. the
/// `GET /api/courses/{courseId}` response), distinct from the generated `LessonSummaryResponse`
/// DTO — see `Course.fromResponse` in `course.dart` for why this app keeps a thin mapping layer
/// instead of depending on OpenAPI Generator's output shape directly.
///
/// `hasQuiz` is the only quiz-related information this app currently has for a lesson summary;
/// the full quiz (questions/answer choices) is only available from `GET /api/lessons/{lessonId}`
/// (see `Lesson` in `lesson.dart`).
class LessonSummary {
  const LessonSummary({
    required this.id,
    required this.title,
    required this.position,
    required this.hasQuiz,
  });

  final String id;
  final String title;
  final int position;
  final bool hasQuiz;

  factory LessonSummary.fromResponse(LessonSummaryResponse response) => LessonSummary(
    id: response.id,
    title: response.title,
    position: response.position,
    hasQuiz: response.hasQuiz,
  );
}

/// App-level representation of `GET /api/courses/{courseId}`: a course's title plus its ordered
/// lesson summaries. Lesson content itself is not part of this model — the Domain `Lesson` has no
/// content/body field, so neither does this app model (see `lesson.dart` for the same note on the
/// Lesson Detail side).
class CourseDetail {
  const CourseDetail({required this.id, required this.title, required this.lessons});

  final String id;
  final String title;
  final List<LessonSummary> lessons;

  factory CourseDetail.fromResponse(CourseDetailResponse response) => CourseDetail(
    id: response.id,
    title: response.title,
    lessons: response.lessons.map(LessonSummary.fromResponse).toList(growable: false),
  );
}
