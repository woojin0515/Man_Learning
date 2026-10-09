import 'package:dio/dio.dart';
import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:man_learning/core/network/dio_provider.dart';
import 'package:man_learning/features/courses/presentation/courses_screen.dart';

import 'fixture_http_client_adapter.dart';

Widget _wrap({required int statusCode, required String body}) {
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
    child: const MaterialApp(home: CoursesScreen()),
  );
}

void main() {
  testWidgets('shows a loading indicator before the response resolves', (tester) async {
    await tester.pumpWidget(_wrap(statusCode: 200, body: '[]'));

    expect(find.text('Loading courses...'), findsOneWidget);

    // Let the pending fetch settle before the test ends — otherwise the widget tree is disposed
    // while the future is still in flight, which flutter_test flags as a pending-timer leak.
    await tester.pumpAndSettle();
  });

  testWidgets('renders course title and lesson count on success', (tester) async {
    await tester.pumpWidget(
      _wrap(
        statusCode: 200,
        body: '[{"id": "1", "title": "AI Fundamentals", "lessonCount": 5}]',
      ),
    );

    await tester.pumpAndSettle();

    expect(find.text('AI Fundamentals'), findsOneWidget);
    expect(find.text('5 lessons'), findsOneWidget);
  });

  testWidgets('renders an empty-state message when there are no courses', (tester) async {
    await tester.pumpWidget(_wrap(statusCode: 200, body: '[]'));

    await tester.pumpAndSettle();

    expect(find.text('No courses available.'), findsOneWidget);
  });

  testWidgets('renders a friendly error message and retry button on failure', (tester) async {
    await tester.pumpWidget(_wrap(statusCode: 500, body: 'boom'));

    await tester.pumpAndSettle();

    expect(find.text('Failed to load courses.'), findsOneWidget);
    expect(find.widgetWithText(FilledButton, 'Retry'), findsOneWidget);
  });
}
