import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../data/courses_repository.dart';
import '../domain/lesson.dart';
import 'courses_providers.dart';

/// Displays a single lesson's *metadata* (title, position, quiz questions/answer choices if any),
/// loaded from `GET /api/lessons/{lessonId}`.
///
/// Important: the Domain `Lesson` entity does not currently model lesson content/body, so this
/// screen never fabricates or displays placeholder lesson content (e.g. "여기에 학습 내용이
/// 들어갑니다."). The UI makes this explicit via the "Lesson metadata" label instead of pretending
/// a content section exists. Quiz *answering* is out of scope for this vertical slice — only quiz
/// questions/answer choice text are shown, read-only.
class LessonDetailScreen extends ConsumerWidget {
  const LessonDetailScreen({required this.lessonId, super.key});

  final String lessonId;

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final lessonAsync = ref.watch(lessonProvider(lessonId));

    return Scaffold(
      appBar: AppBar(title: const Text('Lesson')),
      body: SafeArea(
        child: lessonAsync.when(
          loading: () => const _LessonDetailLoading(),
          error: (error, stackTrace) => _LessonDetailError(
            message: error is CoursesRepositoryException ? error.message : 'Failed to load lesson.',
            isNotFound: error is NotFoundException,
            onRetry: () => ref.invalidate(lessonProvider(lessonId)),
          ),
          data: (lesson) => _LessonDetailBody(lesson: lesson),
        ),
      ),
    );
  }
}

class _LessonDetailLoading extends StatelessWidget {
  const _LessonDetailLoading();

  @override
  Widget build(BuildContext context) {
    return const Center(
      child: Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          CircularProgressIndicator(),
          SizedBox(height: 16),
          Text('Loading lesson...'),
        ],
      ),
    );
  }
}

class _LessonDetailError extends StatelessWidget {
  const _LessonDetailError({required this.message, required this.isNotFound, required this.onRetry});

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
            if (!isNotFound) FilledButton(onPressed: onRetry, child: const Text('Retry')),
          ],
        ),
      ),
    );
  }
}

class _LessonDetailBody extends StatelessWidget {
  const _LessonDetailBody({required this.lesson});

  final Lesson lesson;

  @override
  Widget build(BuildContext context) {
    return LayoutBuilder(
      builder: (context, constraints) {
        final horizontalPadding = constraints.maxWidth > 720 ? 32.0 : 16.0;
        return ListView(
          padding: EdgeInsets.symmetric(horizontal: horizontalPadding, vertical: 16),
          children: [
            Text(lesson.title, style: Theme.of(context).textTheme.headlineSmall),
            const SizedBox(height: 4),
            Text(
              'Lesson ${lesson.position + 1} · metadata only',
              style: Theme.of(context).textTheme.bodyMedium?.copyWith(
                color: Theme.of(context).colorScheme.onSurfaceVariant,
              ),
            ),
            const SizedBox(height: 24),
            if (lesson.quiz == null)
              Text(
                'No quiz for this lesson.',
                style: Theme.of(context).textTheme.bodyLarge,
              )
            else
              _QuizPreview(quiz: lesson.quiz!),
          ],
        );
      },
    );
  }
}

class _QuizPreview extends StatelessWidget {
  const _QuizPreview({required this.quiz});

  final Quiz quiz;

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text('Quiz', style: Theme.of(context).textTheme.titleMedium),
        const SizedBox(height: 8),
        for (final question in quiz.questions) _QuestionCard(question: question),
      ],
    );
  }
}

class _QuestionCard extends StatelessWidget {
  const _QuestionCard({required this.question});

  final Question question;

  @override
  Widget build(BuildContext context) {
    return Card(
      margin: const EdgeInsets.only(bottom: 12),
      child: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(question.text, style: Theme.of(context).textTheme.bodyLarge),
            const SizedBox(height: 8),
            for (final choice in question.answerChoices)
              Padding(
                padding: const EdgeInsets.only(top: 4),
                child: Text(
                  '• ${choice.text}',
                  style: Theme.of(context).textTheme.bodyMedium,
                ),
              ),
          ],
        ),
      ),
    );
  }
}
