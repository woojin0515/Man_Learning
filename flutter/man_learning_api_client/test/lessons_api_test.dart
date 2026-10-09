import 'package:test/test.dart';
import 'package:man_learning_api_client/man_learning_api_client.dart';


/// tests for LessonsApi
void main() {
  final instance = ManLearningApiClient().getLessonsApi();

  group(LessonsApi, () {
    //Future<LessonResponse> apiLessonsLessonIdGet(String lessonId) async
    test('test apiLessonsLessonIdGet', () async {
      // TODO
    });

  });
}
