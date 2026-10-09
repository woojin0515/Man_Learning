//
// AUTO-GENERATED FILE, DO NOT MODIFY!
//

// ignore_for_file: unused_element
import 'package:built_collection/built_collection.dart';
import 'package:man_learning_api_client/src/model/lesson_summary_response.dart';
import 'package:built_value/built_value.dart';
import 'package:built_value/serializer.dart';

part 'course_detail_response.g.dart';

/// CourseDetailResponse
///
/// Properties:
/// * [id] 
/// * [title] 
/// * [lessons] 
@BuiltValue()
abstract class CourseDetailResponse implements Built<CourseDetailResponse, CourseDetailResponseBuilder> {
  @BuiltValueField(wireName: r'id')
  String get id;

  @BuiltValueField(wireName: r'title')
  String get title;

  @BuiltValueField(wireName: r'lessons')
  BuiltList<LessonSummaryResponse> get lessons;

  CourseDetailResponse._();

  factory CourseDetailResponse([void updates(CourseDetailResponseBuilder b)]) = _$CourseDetailResponse;

  @BuiltValueHook(initializeBuilder: true)
  static void _defaults(CourseDetailResponseBuilder b) => b;

  @BuiltValueSerializer(custom: true)
  static Serializer<CourseDetailResponse> get serializer => _$CourseDetailResponseSerializer();
}

class _$CourseDetailResponseSerializer implements PrimitiveSerializer<CourseDetailResponse> {
  @override
  final Iterable<Type> types = const [CourseDetailResponse, _$CourseDetailResponse];

  @override
  final String wireName = r'CourseDetailResponse';

  Iterable<Object?> _serializeProperties(
    Serializers serializers,
    CourseDetailResponse object, {
    FullType specifiedType = FullType.unspecified,
  }) sync* {
    yield r'id';
    yield serializers.serialize(
      object.id,
      specifiedType: const FullType(String),
    );
    yield r'title';
    yield serializers.serialize(
      object.title,
      specifiedType: const FullType(String),
    );
    yield r'lessons';
    yield serializers.serialize(
      object.lessons,
      specifiedType: const FullType(BuiltList, [FullType(LessonSummaryResponse)]),
    );
  }

  @override
  Object serialize(
    Serializers serializers,
    CourseDetailResponse object, {
    FullType specifiedType = FullType.unspecified,
  }) {
    return _serializeProperties(serializers, object, specifiedType: specifiedType).toList();
  }

  void _deserializeProperties(
    Serializers serializers,
    Object serialized, {
    FullType specifiedType = FullType.unspecified,
    required List<Object?> serializedList,
    required CourseDetailResponseBuilder result,
    required List<Object?> unhandled,
  }) {
    for (var i = 0; i < serializedList.length; i += 2) {
      final key = serializedList[i] as String;
      final value = serializedList[i + 1];
      switch (key) {
        case r'id':
          final valueDes = serializers.deserialize(
            value,
            specifiedType: const FullType(String),
          ) as String;
          result.id = valueDes;
          break;
        case r'title':
          final valueDes = serializers.deserialize(
            value,
            specifiedType: const FullType(String),
          ) as String;
          result.title = valueDes;
          break;
        case r'lessons':
          final valueDes = serializers.deserialize(
            value,
            specifiedType: const FullType(BuiltList, [FullType(LessonSummaryResponse)]),
          ) as BuiltList<LessonSummaryResponse>;
          result.lessons.replace(valueDes);
          break;
        default:
          unhandled.add(key);
          unhandled.add(value);
          break;
      }
    }
  }

  @override
  CourseDetailResponse deserialize(
    Serializers serializers,
    Object serialized, {
    FullType specifiedType = FullType.unspecified,
  }) {
    final result = CourseDetailResponseBuilder();
    final serializedList = (serialized as Iterable<Object?>).toList();
    final unhandled = <Object?>[];
    _deserializeProperties(
      serializers,
      serialized,
      specifiedType: specifiedType,
      serializedList: serializedList,
      unhandled: unhandled,
      result: result,
    );
    return result.build();
  }
}


