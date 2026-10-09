import 'package:dio/dio.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:man_learning/features/courses/data/courses_repository.dart';
import 'package:man_learning_api_client/man_learning_api_client.dart';

import 'fixture_http_client_adapter.dart';

/// Builds a [CoursesRepository] backed by the real generated client/serializers, but with its
/// transport faked via [FixtureHttpClientAdapter] — see that file for why. Both [CoursesApi] and
/// [LessonsApi] share the single faked [Dio] instance, matching how [coursesRepositoryProvider]
/// wires them in the real app.
CoursesRepository _repositoryReturning({required int statusCode, required String body}) {
  final dio = Dio(BaseOptions(baseUrl: 'http://localhost:5299'))
    ..httpClientAdapter = FixtureHttpClientAdapter(statusCode: statusCode, body: body);
  final client = ManLearningApiClient(dio: dio);
  return CoursesRepository(client.getCoursesApi(), client.getLessonsApi());
}

void main() {
  group('CoursesRepository.getCourses', () {
    test('maps a successful response to Course models', () async {
      final repository = _repositoryReturning(
        statusCode: 200,
        body: '''
        [
          {"id": "8f1a1b1e-1111-4a1a-9a1a-000000000001", "title": "AI Fundamentals", "lessonCount": 5},
          {"id": "8f1a1b1e-1111-4a1a-9a1a-000000000002", "title": "Machine Learning", "lessonCount": 8}
        ]
        ''',
      );

      final courses = await repository.getCourses();

      expect(courses, hasLength(2));
      expect(courses[0].id, '8f1a1b1e-1111-4a1a-9a1a-000000000001');
      expect(courses[0].title, 'AI Fundamentals');
      expect(courses[0].lessonCount, 5);
      expect(courses[1].title, 'Machine Learning');
      expect(courses[1].lessonCount, 8);
    });

    test('returns an empty list when the API returns no courses', () async {
      final repository = _repositoryReturning(statusCode: 200, body: '[]');

      final courses = await repository.getCourses();

      expect(courses, isEmpty);
    });

    test('wraps transport failures in CoursesRepositoryException', () async {
      final repository = _repositoryReturning(statusCode: 500, body: 'Internal Server Error');

      await expectLater(repository.getCourses(), throwsA(isA<CoursesRepositoryException>()));
    });
  });

  group('CoursesRepository.getCourse', () {
    test('maps a successful response to a CourseDetail with lesson summaries', () async {
      final repository = _repositoryReturning(
        statusCode: 200,
        body: '''
        {
          "id": "8f1a1b1e-1111-4a1a-9a1a-000000000001",
          "title": "AI Fundamentals",
          "lessons": [
            {"id": "8f1a1b1e-2222-4a1a-9a1a-000000000001", "title": "What is AI?", "position": 0, "hasQuiz": false},
            {"id": "8f1a1b1e-2222-4a1a-9a1a-000000000002", "title": "Types of AI", "position": 1, "hasQuiz": true}
          ]
        }
        ''',
      );

      final course = await repository.getCourse('8f1a1b1e-1111-4a1a-9a1a-000000000001');

      expect(course.title, 'AI Fundamentals');
      expect(course.lessons, hasLength(2));
      expect(course.lessons[0].hasQuiz, isFalse);
      expect(course.lessons[1].hasQuiz, isTrue);
    });

    test('throws NotFoundException on a 404 response', () async {
      final repository = _repositoryReturning(
        statusCode: 404,
        body: '{"title": "Not Found", "status": 404}',
      );

      await expectLater(
        repository.getCourse('00000000-0000-0000-0000-000000000000'),
        throwsA(isA<NotFoundException>()),
      );
    });

    test('wraps other transport failures in CoursesRepositoryException', () async {
      final repository = _repositoryReturning(statusCode: 500, body: 'Internal Server Error');

      await expectLater(
        repository.getCourse('8f1a1b1e-1111-4a1a-9a1a-000000000001'),
        throwsA(isA<CoursesRepositoryException>()),
      );
    });
  });

  group('CoursesRepository.getLesson', () {
    test('maps a successful response without a quiz', () async {
      final repository = _repositoryReturning(
        statusCode: 200,
        body: '''
        {
          "id": "8f1a1b1e-2222-4a1a-9a1a-000000000001",
          "title": "What is AI?",
          "position": 0,
          "quiz": null
        }
        ''',
      );

      final lesson = await repository.getLesson('8f1a1b1e-2222-4a1a-9a1a-000000000001');

      expect(lesson.title, 'What is AI?');
      expect(lesson.position, 0);
      expect(lesson.quiz, isNull);
    });

    test('maps a successful response with a quiz and its questions/answer choices', () async {
      final repository = _repositoryReturning(
        statusCode: 200,
        body: '''
        {
          "id": "8f1a1b1e-2222-4a1a-9a1a-000000000002",
          "title": "Types of AI",
          "position": 1,
          "quiz": {
            "id": "8f1a1b1e-5555-4a1a-9a1a-000000000001",
            "questions": [
              {
                "id": "8f1a1b1e-3333-4a1a-9a1a-000000000001",
                "text": "What is Narrow AI?",
                "answerChoices": [
                  {"id": "8f1a1b1e-4444-4a1a-9a1a-000000000001", "text": "Task-specific AI"},
                  {"id": "8f1a1b1e-4444-4a1a-9a1a-000000000002", "text": "General AI"}
                ]
              }
            ]
          }
        }
        ''',
      );

      final lesson = await repository.getLesson('8f1a1b1e-2222-4a1a-9a1a-000000000002');

      expect(lesson.quiz, isNotNull);
      expect(lesson.quiz!.questions, hasLength(1));
      expect(lesson.quiz!.questions.single.answerChoices, hasLength(2));
    });

    test('throws NotFoundException on a 404 response', () async {
      final repository = _repositoryReturning(
        statusCode: 404,
        body: '{"title": "Not Found", "status": 404}',
      );

      await expectLater(
        repository.getLesson('00000000-0000-0000-0000-000000000000'),
        throwsA(isA<NotFoundException>()),
      );
    });

    test('wraps other transport failures in CoursesRepositoryException', () async {
      final repository = _repositoryReturning(statusCode: 500, body: 'Internal Server Error');

      await expectLater(
        repository.getLesson('8f1a1b1e-2222-4a1a-9a1a-000000000001'),
        throwsA(isA<CoursesRepositoryException>()),
      );
    });
  });
}
