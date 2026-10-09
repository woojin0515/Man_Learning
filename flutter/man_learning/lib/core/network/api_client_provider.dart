import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:man_learning_api_client/man_learning_api_client.dart';

import 'dio_provider.dart';

/// Wraps the generated [ManLearningApiClient] (OpenAPI Generator 7.25.0, dart-dio, built_value —
/// see docs/spikes/openapi-dart-client-generation.md) around the app's single [dioProvider]
/// instance, so every generated API ([CoursesApi], [LessonsApi]) shares one Dio configuration
/// instead of each constructing its own.
final apiClientProvider = Provider<ManLearningApiClient>((ref) {
  return ManLearningApiClient(dio: ref.watch(dioProvider));
});

/// The generated `GET /api/courses` / `GET /api/courses/{courseId}` client. Feature repositories
/// depend on this provider, not on [apiClientProvider] or [dioProvider] directly — UI code must
/// never reach past a repository to call a generated API client itself.
final coursesApiProvider = Provider<CoursesApi>((ref) {
  return ref.watch(apiClientProvider).getCoursesApi();
});

/// The generated `GET /api/lessons/{lessonId}` client. Same rule as [coursesApiProvider]: only
/// [CoursesRepository] (or an equivalent feature repository) may use this directly.
final lessonsApiProvider = Provider<LessonsApi>((ref) {
  return ref.watch(apiClientProvider).getLessonsApi();
});
