import 'package:dio/dio.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:man_learning/core/network/dio_provider.dart';
import 'package:man_learning/features/courses/domain/course.dart';
import 'package:man_learning/features/courses/domain/course_detail.dart';
import 'package:man_learning/features/courses/domain/lesson.dart';
import 'package:man_learning/features/courses/presentation/courses_providers.dart';

import 'fixture_http_client_adapter.dart';

/// Overrides [dioProvider] so [coursesProvider] (and everything it depends on:
/// [apiClientProvider] → [coursesApiProvider] → [coursesRepositoryProvider]) runs against a faked
/// transport instead of a real network call, while still exercising the real provider wiring.
///
/// Automatic retry (Riverpod 3's default behaviour — up to 10 attempts with exponential backoff)
/// is disabled here: it's the right default for the real app, but would make the "transport
/// failure" test below retry for several seconds before settling into an error state.
ProviderContainer _containerReturning({required int statusCode, required String body}) {
  final container = ProviderContainer(
    retry: (retryCount, error) => null,
    overrides: [
      dioProvider.overrideWithValue(
        Dio(BaseOptions(baseUrl: 'http://localhost:5299'))
          ..httpClientAdapter = FixtureHttpClientAdapter(statusCode: statusCode, body: body),
      ),
    ],
  );
  addTearDown(container.dispose);
  return container;
}

void main() {
  group('coursesProvider', () {
    test('starts in loading state', () async {
      final container = _containerReturning(statusCode: 200, body: '[]');

      final initial = container.read(coursesProvider);

      expect(initial, isA<AsyncLoading<List<Course>>>());

      // Let the pending fetch settle before teardown disposes the container — otherwise the
      // in-flight future completes against an already-disposed container and leaks an
      // uncaught "Bad state" error into later tests.
      await container.read(coursesProvider.future);
    });

    test('resolves to AsyncData with mapped courses on success', () async {
      final container = _containerReturning(
        statusCode: 200,
        body: '[{"id": "1", "title": "AI Fundamentals", "lessonCount": 5}]',
      );

      final courses = await container.read(coursesProvider.future);

      expect(courses, hasLength(1));
      expect(courses.single.title, 'AI Fundamentals');
      expect(container.read(coursesProvider).hasValue, isTrue);
    });

    test('resolves to AsyncError on transport failure', () async {
      final container = _containerReturning(statusCode: 500, body: 'error');

      await expectLater(container.read(coursesProvider.future), throwsA(anything));
      expect(container.read(coursesProvider).hasError, isTrue);
    });
  });

  group('courseProvider', () {
    test('resolves to AsyncData with a mapped CourseDetail on success', () async {
      final container = _containerReturning(
        statusCode: 200,
        body: '''
        {
          "id": "1",
          "title": "AI Fundamentals",
          "lessons": [
            {"id": "l1", "title": "What is AI?", "position": 0, "hasQuiz": false}
          ]
        }
        ''',
      );

      final course = await container.read(courseProvider('1').future);

      expect(course, isA<CourseDetail>());
      expect(course.title, 'AI Fundamentals');
      expect(course.lessons.single.title, 'What is AI?');
    });

    test('resolves to AsyncError on a 404 response', () async {
      final container = _containerReturning(statusCode: 404, body: '{"status": 404}');

      await expectLater(container.read(courseProvider('missing').future), throwsA(anything));
      expect(container.read(courseProvider('missing')).hasError, isTrue);
    });
  });

  group('lessonProvider', () {
    test('resolves to AsyncData with a mapped Lesson on success', () async {
      final container = _containerReturning(
        statusCode: 200,
        body: '{"id": "l1", "title": "What is AI?", "position": 0, "quiz": null}',
      );

      final lesson = await container.read(lessonProvider('l1').future);

      expect(lesson, isA<Lesson>());
      expect(lesson.title, 'What is AI?');
      expect(lesson.quiz, isNull);
    });

    test('resolves to AsyncError on a 404 response', () async {
      final container = _containerReturning(statusCode: 404, body: '{"status": 404}');

      await expectLater(container.read(lessonProvider('missing').future), throwsA(anything));
      expect(container.read(lessonProvider('missing')).hasError, isTrue);
    });
  });
}
