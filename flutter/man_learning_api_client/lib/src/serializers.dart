//
// AUTO-GENERATED FILE, DO NOT MODIFY!
//

// ignore_for_file: unused_import

import 'package:one_of_serializer/any_of_serializer.dart';
import 'package:one_of_serializer/one_of_serializer.dart';
import 'package:built_collection/built_collection.dart';
import 'package:built_value/json_object.dart';
import 'package:built_value/serializer.dart';
import 'package:built_value/standard_json_plugin.dart';
import 'package:built_value/iso_8601_date_time_serializer.dart';
import 'package:man_learning_api_client/src/date_serializer.dart';
import 'package:man_learning_api_client/src/model/date.dart';

import 'package:man_learning_api_client/src/model/answer_choice_response.dart';
import 'package:man_learning_api_client/src/model/course_detail_response.dart';
import 'package:man_learning_api_client/src/model/course_response.dart';
import 'package:man_learning_api_client/src/model/lesson_response.dart';
import 'package:man_learning_api_client/src/model/lesson_summary_response.dart';
import 'package:man_learning_api_client/src/model/problem_details.dart';
import 'package:man_learning_api_client/src/model/question_response.dart';
import 'package:man_learning_api_client/src/model/quiz_response.dart';

part 'serializers.g.dart';

@SerializersFor([
  AnswerChoiceResponse,
  CourseDetailResponse,
  CourseResponse,
  LessonResponse,
  LessonSummaryResponse,
  ProblemDetails,
  QuestionResponse,
  QuizResponse,
])
Serializers serializers = (_$serializers.toBuilder()
      ..addBuilderFactory(
        const FullType(BuiltList, [FullType(LessonSummaryResponse)]),
        () => ListBuilder<LessonSummaryResponse>(),
      )
      ..addBuilderFactory(
        const FullType(BuiltList, [FullType(AnswerChoiceResponse)]),
        () => ListBuilder<AnswerChoiceResponse>(),
      )
      ..addBuilderFactory(
        const FullType(BuiltList, [FullType(QuestionResponse)]),
        () => ListBuilder<QuestionResponse>(),
      )
      ..addBuilderFactory(
        const FullType(BuiltList, [FullType(CourseResponse)]),
        () => ListBuilder<CourseResponse>(),
      )
      ..add(const OneOfSerializer())
      ..add(const AnyOfSerializer())
      ..add(const DateSerializer())
      ..add(Iso8601DateTimeSerializer())
    ).build();

Serializers standardSerializers =
    (serializers.toBuilder()..addPlugin(StandardJsonPlugin())).build();
