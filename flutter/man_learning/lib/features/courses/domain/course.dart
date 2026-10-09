import 'package:man_learning_api_client/man_learning_api_client.dart';

/// App-level representation of a course, distinct from the generated `CourseResponse` DTO.
///
/// Decision (recorded here rather than in a separate ADR, since this is a small implementation
/// detail, not an architectural decision — see "Decision" notes in
/// `docs/spikes/openapi-dart-client-generation.md` for the broader generated-client policy):
/// keep this one thin mapping layer even though `Course` and `CourseResponse` currently have
/// identical fields. The cost is one small class + one factory method; the benefit is that
/// everything above `CoursesRepository` (Riverpod providers, widgets, future features) depends on
/// a type this app owns, not on OpenAPI Generator's output shape. If the generated model's field
/// names, nullability, or wire types change on a future `dotnet` OpenAPI Generator regeneration,
/// only `Course.fromResponse` needs to change — UI code does not. This is intentionally the only
/// layer added; no separate "data model" vs "domain model" split was introduced beyond this.
class Course {
  const Course({required this.id, required this.title, required this.lessonCount});

  final String id;
  final String title;
  final int lessonCount;

  factory Course.fromResponse(CourseResponse response) => Course(
    id: response.id,
    title: response.title,
    lessonCount: response.lessonCount,
  );
}
