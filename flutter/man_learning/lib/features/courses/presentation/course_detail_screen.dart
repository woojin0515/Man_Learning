import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../data/courses_repository.dart';
import '../domain/course_detail.dart';
import 'courses_providers.dart';

/// Displays a single course's title and its ordered lesson list, loaded from
/// `GET /api/courses/{courseId}`. Renders loading/error/not-found/data states; selecting a lesson
/// navigates to `/courses/:courseId/lessons/:lessonId` ([LessonDetailScreen]).
///
/// Only lesson metadata currently modeled by the Domain is shown (title, position, whether a quiz
/// exists) — lesson content itself is a separate future architectural decision (see `lesson.dart`).
class CourseDetailScreen extends ConsumerWidget {
  const CourseDetailScreen({required this.courseId, super.key});

  final String courseId;

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final courseAsync = ref.watch(courseProvider(courseId));

    return Scaffold(
      appBar: AppBar(title: const Text('Course')),
      body: SafeArea(
        child: courseAsync.when(
          loading: () => const _CourseDetailLoading(),
          error: (error, stackTrace) => _CourseDetailError(
            message: error is CoursesRepositoryException ? error.message : 'Failed to load course.',
            isNotFound: error is NotFoundException,
            onRetry: () => ref.invalidate(courseProvider(courseId)),
          ),
          data: (course) => _CourseDetailBody(course: course),
        ),
      ),
    );
  }
}

class _CourseDetailLoading extends StatelessWidget {
  const _CourseDetailLoading();

  @override
  Widget build(BuildContext context) {
    return const Center(
      child: Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          CircularProgressIndicator(),
          SizedBox(height: 16),
          Text('Loading course...'),
        ],
      ),
    );
  }
}

class _CourseDetailError extends StatelessWidget {
  const _CourseDetailError({required this.message, required this.isNotFound, required this.onRetry});

  final String message;
  final bool isNotFound;
  final VoidCallback onRetry;

  @override
  Widget build(BuildContext context) {
    return Center(
      child: Padding(
        padding: const EdgeInsets.all(24),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Icon(Icons.error_outline, size: 40, color: Theme.of(context).colorScheme.error),
            const SizedBox(height: 12),
            Text(message, style: Theme.of(context).textTheme.bodyLarge, textAlign: TextAlign.center),
            const SizedBox(height: 16),
            // A missing course is not a transient failure, so there is nothing to retry — only
            // show the retry action for genuine (e.g. transport) errors.
            if (!isNotFound) FilledButton(onPressed: onRetry, child: const Text('Retry')),
          ],
        ),
      ),
    );
  }
}

class _CourseDetailBody extends StatelessWidget {
  const _CourseDetailBody({required this.course});

  final CourseDetail course;

  @override
  Widget build(BuildContext context) {
    if (course.lessons.isEmpty) {
      return Center(
        child: Padding(
          padding: const EdgeInsets.all(24),
          child: Text(
            '${course.title}\n\nNo lessons available yet.',
            style: Theme.of(context).textTheme.bodyLarge,
            textAlign: TextAlign.center,
          ),
        ),
      );
    }

    return LayoutBuilder(
      builder: (context, constraints) {
        final horizontalPadding = constraints.maxWidth > 720 ? 32.0 : 16.0;
        return ListView(
          padding: EdgeInsets.symmetric(horizontal: horizontalPadding, vertical: 16),
          children: [
            Text(course.title, style: Theme.of(context).textTheme.headlineSmall),
            const SizedBox(height: 16),
            Text('Lessons', style: Theme.of(context).textTheme.titleMedium),
            const SizedBox(height: 8),
            for (final lesson in course.lessons)
              _LessonListTile(courseId: course.id, lesson: lesson),
          ],
        );
      },
    );
  }
}

class _LessonListTile extends StatelessWidget {
  const _LessonListTile({required this.courseId, required this.lesson});

  final String courseId;
  final LessonSummary lesson;

  @override
  Widget build(BuildContext context) {
    return Card(
      child: ListTile(
        leading: CircleAvatar(child: Text('${lesson.position + 1}')),
        title: Text(lesson.title),
        trailing: lesson.hasQuiz ? const Icon(Icons.quiz_outlined) : null,
        onTap: () => context.go('/courses/$courseId/lessons/${lesson.id}'),
      ),
    );
  }
}
