import 'package:test/test.dart';
import 'package:man_learning_api_client/man_learning_api_client.dart';


/// tests for CoursesApi
void main() {
  final instance = ManLearningApiClient().getCoursesApi();

  group(CoursesApi, () {
    //Future<CourseDetailResponse> apiCoursesCourseIdGet(String courseId) async
    test('test apiCoursesCourseIdGet', () async {
      // TODO
    });

    //Future<BuiltList<CourseResponse>> apiCoursesGet() async
    test('test apiCoursesGet', () async {
      // TODO
    });

  });
}
