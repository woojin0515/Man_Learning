import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../data/courses_repository.dart';
import '../domain/course.dart';
import 'courses_providers.dart';

/// Displays the course catalog loaded from `GET /api/courses`. Renders four states: loading,
/// success with courses, success with an empty list, and error — none of them hard-code course
/// data; everything comes from [coursesProvider].
class CoursesScreen extends ConsumerWidget {
  const CoursesScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final coursesAsync = ref.watch(coursesProvider);

    return Scaffold(
      appBar: AppBar(title: const Text('Courses')),
      body: SafeArea(
        child: coursesAsync.when(
          loading: () => const _CoursesLoading(),
          error: (error, stackTrace) => _CoursesError(
            message: error is CoursesRepositoryException ? error.message : 'Failed to load courses.',
            onRetry: () => ref.invalidate(coursesProvider),
          ),
          data: (courses) => courses.isEmpty
              ? const _CoursesEmpty()
              : _CoursesList(courses: courses),
        ),
      ),
    );
  }
}

class _CoursesLoading extends StatelessWidget {
  const _CoursesLoading();

  @override
  Widget build(BuildContext context) {
    return const Center(
      child: Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          CircularProgressIndicator(),
          SizedBox(height: 16),
          Text('Loading courses...'),
        ],
      ),
    );
  }
}

class _CoursesEmpty extends StatelessWidget {
  const _CoursesEmpty();

  @override
  Widget build(BuildContext context) {
    return Center(
      child: Padding(
        padding: const EdgeInsets.all(24),
        child: Text(
          'No courses available.',
          style: Theme.of(context).textTheme.bodyLarge,
          textAlign: TextAlign.center,
        ),
      ),
    );
  }
}

class _CoursesError extends StatelessWidget {
  const _CoursesError({required this.message, required this.onRetry});

  final String message;
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
            FilledButton(onPressed: onRetry, child: const Text('Retry')),
          ],
        ),
      ),
    );
  }
}

class _CoursesList extends StatelessWidget {
  const _CoursesList({required this.courses});

  final List<Course> courses;

  @override
  Widget build(BuildContext context) {
    return LayoutBuilder(
      builder: (context, constraints) {
        final horizontalPadding = constraints.maxWidth > 720 ? 32.0 : 16.0;
        return ListView.separated(
          padding: EdgeInsets.symmetric(horizontal: horizontalPadding, vertical: 16),
          itemCount: courses.length,
          separatorBuilder: (context, index) => const SizedBox(height: 12),
          itemBuilder: (context, index) => _CourseCard(course: courses[index]),
        );
      },
    );
  }
}

class _CourseCard extends StatelessWidget {
  const _CourseCard({required this.course});

  final Course course;

  @override
  Widget build(BuildContext context) {
    final lessonLabel = course.lessonCount == 1 ? '1 lesson' : '${course.lessonCount} lessons';
    return Card(
      child: InkWell(
        onTap: () => context.go('/courses/${course.id}'),
        child: Padding(
          padding: const EdgeInsets.all(16),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                course.title,
                style: Theme.of(context).textTheme.titleMedium,
                overflow: TextOverflow.ellipsis,
                maxLines: 2,
              ),
              const SizedBox(height: 4),
              Text(
                lessonLabel,
                style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                  color: Theme.of(context).colorScheme.onSurfaceVariant,
                ),
                semanticsLabel: '$lessonLabel in ${course.title}',
              ),
            ],
          ),
        ),
      ),
    );
  }
}
