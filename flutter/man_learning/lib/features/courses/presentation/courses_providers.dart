import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../../core/network/api_client_provider.dart';
import '../data/courses_repository.dart';
import '../domain/course.dart';
import '../domain/course_detail.dart';
import '../domain/lesson.dart';

/// Injects [CoursesRepository] wired to the generated `CoursesApi`/`LessonsApi` (via
/// [coursesApiProvider]/[lessonsApiProvider]). Widgets and other providers depend on this, never
/// on the generated API providers directly.
final coursesRepositoryProvider = Provider<CoursesRepository>((ref) {
  return CoursesRepository(ref.watch(coursesApiProvider), ref.watch(lessonsApiProvider));
});

/// Loads the course catalog from `GET /api/courses`. A plain [FutureProvider] already exposes
/// Riverpod's `AsyncValue<List<Course>>` (loading/data/error) without needing the
/// `riverpod_generator`/`build_runner` codegen package — this one endpoint does not justify that
/// extra dependency/codegen step. `CoursesScreen` renders each of the three `AsyncValue` states,
/// plus a fourth "empty" case when the list loads successfully but has zero courses.
final coursesProvider = FutureProvider<List<Course>>((ref) {
  return ref.watch(coursesRepositoryProvider).getCourses();
});

/// Loads a single course's detail (title + ordered lesson summaries) from
/// `GET /api/courses/{courseId}`. A `.family` [FutureProvider] (not codegen, matching
/// [coursesProvider]'s style) keyed by `courseId` so `CourseDetailScreen` can `ref.watch` it per
/// route parameter.
final courseProvider = FutureProvider.family<CourseDetail, String>((ref, courseId) {
  return ref.watch(coursesRepositoryProvider).getCourse(courseId);
});

/// Loads a single lesson's metadata (title, position, quiz) from `GET /api/lessons/{lessonId}`.
/// Same `.family` approach as [courseProvider], keyed by `lessonId`.
final lessonProvider = FutureProvider.family<Lesson, String>((ref, lessonId) {
  return ref.watch(coursesRepositoryProvider).getLesson(lessonId);
});
