import 'package:go_router/go_router.dart';

import '../features/courses/presentation/course_detail_screen.dart';
import '../features/courses/presentation/courses_screen.dart';
import '../features/courses/presentation/lesson_detail_screen.dart';

/// Route table for the Courses → Course Detail → Lesson Detail vertical slice: `/courses` renders
/// [CoursesScreen], `/courses/:courseId` renders [CourseDetailScreen], and
/// `/courses/:courseId/lessons/:lessonId` renders [LessonDetailScreen]. The root route (`/`)
/// redirects to `/courses`. No auth redirects or nested (shell) navigation yet — those are
/// deferred to when ADR-0007 (authentication) and further features exist.
final appRouter = GoRouter(
  initialLocation: '/courses',
  routes: [
    GoRoute(path: '/', redirect: (context, state) => '/courses'),
    GoRoute(path: '/courses', builder: (context, state) => const CoursesScreen()),
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
