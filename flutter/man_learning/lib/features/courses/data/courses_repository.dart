import 'package:built_collection/built_collection.dart';
import 'package:dio/dio.dart';
import 'package:man_learning_api_client/man_learning_api_client.dart';

import '../domain/course.dart';
import '../domain/course_detail.dart';
import '../domain/lesson.dart';

/// Thrown by [CoursesRepository] when `GET /api/courses` fails. Wraps the underlying [DioException]
/// so callers (Riverpod providers, widgets) can show a user-friendly message without needing to
/// know about Dio or HTTP status codes.
class CoursesRepositoryException implements Exception {
  const CoursesRepositoryException(this.message, {this.cause});

  final String message;
  final Object? cause;

  @override
  String toString() => message;
}

/// Thrown by [CoursesRepository.getCourse]/[CoursesRepository.getLesson] specifically when the
/// API responds `404 Not Found` — i.e. the course/lesson id does not exist. Kept distinct from
/// the generic [CoursesRepositoryException] so widgets can show a "not found" message instead of
/// a generic "failed to load" message, and so a caller that does want to treat it generically
/// still can (this is a subtype).
class NotFoundException extends CoursesRepositoryException {
  const NotFoundException(super.message, {super.cause});
}

/// Boundary between the app and the courses/lessons endpoints (`GET /api/courses`,
/// `GET /api/courses/{courseId}`, `GET /api/lessons/{lessonId}`). This is the *only* place in the
/// app that is allowed to call [CoursesApi]/[LessonsApi] directly (ADR-0006/ADR-0009's layering:
/// UI → Riverpod → Repository → generated API client → Dio). It also converts the generated
/// response DTOs into this app's own domain models (see `Course.fromResponse` for why) and
/// translates transport-level failures into repository-level exceptions instead of leaking
/// `DioException` upward.
class CoursesRepository {
  const CoursesRepository(this._coursesApi, this._lessonsApi);

  final CoursesApi _coursesApi;
  final LessonsApi _lessonsApi;

  Future<List<Course>> getCourses() async {
    try {
      final response = await _coursesApi.apiCoursesGet();
      final courses = response.data ?? BuiltList<CourseResponse>();
      return courses.map(Course.fromResponse).toList(growable: false);
    } on DioException catch (error) {
      throw CoursesRepositoryException('Failed to load courses.', cause: error);
    }
  }

  /// Loads a single course's detail (title + ordered lesson summaries) via
  /// `GET /api/courses/{courseId}`. Throws [NotFoundException] when `courseId` does not exist.
  Future<CourseDetail> getCourse(String courseId) async {
    try {
      final response = await _coursesApi.apiCoursesCourseIdGet(courseId: courseId);
      final body = response.data;
      if (body == null) {
        throw const CoursesRepositoryException('Failed to load course: empty response.');
      }
      return CourseDetail.fromResponse(body);
    } on DioException catch (error) {
      if (error.response?.statusCode == 404) {
        throw NotFoundException('Course not found.', cause: error);
      }
      throw CoursesRepositoryException('Failed to load course.', cause: error);
    }
  }

  /// Loads a single lesson's metadata (title, position, quiz questions/answer choices if any) via
  /// `GET /api/lessons/{lessonId}`. Throws [NotFoundException] when `lessonId` does not exist.
  ///
  /// Note: the response intentionally has no lesson content/body — see [Lesson]'s doc comment.
  Future<Lesson> getLesson(String lessonId) async {
    try {
      final response = await _lessonsApi.apiLessonsLessonIdGet(lessonId: lessonId);
      final body = response.data;
      if (body == null) {
        throw const CoursesRepositoryException('Failed to load lesson: empty response.');
      }
      return Lesson.fromResponse(body);
    } on DioException catch (error) {
      if (error.response?.statusCode == 404) {
        throw NotFoundException('Lesson not found.', cause: error);
      }
      throw CoursesRepositoryException('Failed to load lesson.', cause: error);
    }
  }
}
