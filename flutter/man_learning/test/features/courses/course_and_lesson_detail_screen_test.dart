import 'package:dio/dio.dart';
import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:go_router/go_router.dart';
import 'package:man_learning/core/network/dio_provider.dart';
import 'package:man_learning/features/courses/presentation/course_detail_screen.dart';
import 'package:man_learning/features/courses/presentation/lesson_detail_screen.dart';

import 'fixture_http_client_adapter.dart';

Widget _wrapWithRouter({required int statusCode, required String body}) {
  final router = GoRouter(
    initialLocation: '/courses/course-1',
    routes: [
      GoRoute(
        path: '/courses/:courseId',
        builder: (context, state) =>
            CourseDetailScreen(courseId: state.pathParameters['courseId']!),
      ),
      GoRoute(
        path: '/courses/:courseId/lessons/:lessonId',
        builder: (context, state) =>
            LessonDetailScreen(lessonId: state.pathParameters['lessonId']!),
      ),
    ],
  );

  return ProviderScope(
    // Disable Riverpod 3's automatic retry (see courses_providers_test.dart) so failed-request
    // tests settle into an error state immediately instead of retrying for several seconds.
    retry: (retryCount, error) => null,
    overrides: [
      dioProvider.overrideWithValue(
        Dio(BaseOptions(baseUrl: 'http://localhost:5299'))
          ..httpClientAdapter = FixtureHttpClientAdapter(statusCode: statusCode, body: body),
      ),
    ],
    child: MaterialApp.router(routerConfig: router),
  );
}

void main() {
  group('CourseDetailScreen', () {
    testWidgets('renders course title, lesson list, and quiz indicator', (tester) async {
      await tester.pumpWidget(
        _wrapWithRouter(
          statusCode: 200,
          body: '''
          {
            "id": "course-1",
            "title": "AI Fundamentals",
            "lessons": [
              {"id": "lesson-1", "title": "What is AI?", "position": 0, "hasQuiz": false},
              {"id": "lesson-2", "title": "Types of AI", "position": 1, "hasQuiz": true}
            ]
          }
          ''',
        ),
      );

      await tester.pumpAndSettle();

      expect(find.text('AI Fundamentals'), findsOneWidget);
      expect(find.text('What is AI?'), findsOneWidget);
      expect(find.text('Types of AI'), findsOneWidget);
      expect(find.byIcon(Icons.quiz_outlined), findsOneWidget);
    });

    testWidgets('selecting a lesson navigates to the Lesson Detail screen', (tester) async {
      // Every request receives the same canned course-detail body, but LessonDetailScreen only
      // reads the `quiz` field from it, which matches between both screens here since this
      // fixture only asserts that navigation occurred, not the lesson's own content.
      await tester.pumpWidget(
        _wrapWithRouter(
          statusCode: 200,
          body: '''
          {
            "id": "course-1",
            "title": "AI Fundamentals",
            "lessons": [
              {"id": "lesson-1", "title": "What is AI?", "position": 0, "hasQuiz": false}
            ]
          }
          ''',
        ),
      );
      await tester.pumpAndSettle();

      await tester.tap(find.text('What is AI?'));
      await tester.pumpAndSettle();

      expect(find.byType(LessonDetailScreen), findsOneWidget);
    });

    testWidgets('renders a not-found message without a retry button on 404', (tester) async {
      await tester.pumpWidget(_wrapWithRouter(statusCode: 404, body: '{"status": 404}'));

      await tester.pumpAndSettle();

      expect(find.text('Course not found.'), findsOneWidget);
      expect(find.widgetWithText(FilledButton, 'Retry'), findsNothing);
    });
  });

  group('LessonDetailScreen', () {
    testWidgets('renders lesson title and metadata without fabricated content', (tester) async {
      final router = GoRouter(
        initialLocation: '/courses/course-1/lessons/lesson-1',
        routes: [
          GoRoute(
            path: '/courses/:courseId/lessons/:lessonId',
            builder: (context, state) =>
                LessonDetailScreen(lessonId: state.pathParameters['lessonId']!),
          ),
        ],
      );

      await tester.pumpWidget(
        ProviderScope(
          retry: (retryCount, error) => null,
          overrides: [
            dioProvider.overrideWithValue(
              Dio(BaseOptions(baseUrl: 'http://localhost:5299'))
                ..httpClientAdapter = FixtureHttpClientAdapter(
                  statusCode: 200,
                  body: '{"id": "lesson-1", "title": "What is AI?", "position": 0, "quiz": null}',
                ),
            ),
          ],
          child: MaterialApp.router(routerConfig: router),
        ),
      );

      await tester.pumpAndSettle();

      expect(find.text('What is AI?'), findsOneWidget);
      expect(find.textContaining('metadata only'), findsOneWidget);
      expect(find.text('No quiz for this lesson.'), findsOneWidget);
      // No fabricated lesson body/content should ever be rendered.
      expect(find.textContaining('학습 내용'), findsNothing);
    });

    testWidgets('renders quiz questions and answer choices when a quiz exists', (tester) async {
      final router = GoRouter(
        initialLocation: '/courses/course-1/lessons/lesson-2',
        routes: [
          GoRoute(
            path: '/courses/:courseId/lessons/:lessonId',
            builder: (context, state) =>
                LessonDetailScreen(lessonId: state.pathParameters['lessonId']!),
          ),
        ],
      );

      await tester.pumpWidget(
        ProviderScope(
          retry: (retryCount, error) => null,
          overrides: [
            dioProvider.overrideWithValue(
              Dio(BaseOptions(baseUrl: 'http://localhost:5299'))
                ..httpClientAdapter = FixtureHttpClientAdapter(
                  statusCode: 200,
                  body: '''
                  {
                    "id": "lesson-2",
                    "title": "Types of AI",
                    "position": 1,
                    "quiz": {
                      "id": "quiz-1",
                      "questions": [
                        {
                          "id": "q1",
                          "text": "What is Narrow AI?",
                          "answerChoices": [
                            {"id": "a1", "text": "Task-specific AI"},
                            {"id": "a2", "text": "General AI"}
                          ]
                        }
                      ]
                    }
                  }
                  ''',
                ),
            ),
          ],
          child: MaterialApp.router(routerConfig: router),
        ),
      );

      await tester.pumpAndSettle();

      expect(find.text('What is Narrow AI?'), findsOneWidget);
      expect(find.text('• Task-specific AI'), findsOneWidget);
      expect(find.text('• General AI'), findsOneWidget);
    });
  });
}
