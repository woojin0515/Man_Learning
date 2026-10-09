//
// AUTO-GENERATED FILE, DO NOT MODIFY!
//

// ignore_for_file: unused_element
import 'package:man_learning_api_client/src/model/quiz_response.dart';
import 'package:built_value/built_value.dart';
import 'package:built_value/serializer.dart';

part 'lesson_response.g.dart';

/// LessonResponse
///
/// Properties:
/// * [id] 
/// * [title] 
/// * [position] 
/// * [quiz] 
@BuiltValue()
abstract class LessonResponse implements Built<LessonResponse, LessonResponseBuilder> {
  @BuiltValueField(wireName: r'id')
  String get id;

  @BuiltValueField(wireName: r'title')
  String get title;

  @BuiltValueField(wireName: r'position')
  int get position;

  @BuiltValueField(wireName: r'quiz')
  QuizResponse? get quiz;

  LessonResponse._();

  factory LessonResponse([void updates(LessonResponseBuilder b)]) = _$LessonResponse;

  @BuiltValueHook(initializeBuilder: true)
  static void _defaults(LessonResponseBuilder b) => b;

  @BuiltValueSerializer(custom: true)
  static Serializer<LessonResponse> get serializer => _$LessonResponseSerializer();
}

class _$LessonResponseSerializer implements PrimitiveSerializer<LessonResponse> {
  @override
  final Iterable<Type> types = const [LessonResponse, _$LessonResponse];

  @override
  final String wireName = r'LessonResponse';

  Iterable<Object?> _serializeProperties(
    Serializers serializers,
    LessonResponse object, {
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
    yield r'position';
    yield serializers.serialize(
      object.position,
      specifiedType: const FullType(int),
    );
    yield r'quiz';
    yield object.quiz == null ? null : serializers.serialize(
      object.quiz,
      specifiedType: const FullType.nullable(QuizResponse),
    );
  }

  @override
  Object serialize(
    Serializers serializers,
    LessonResponse object, {
    FullType specifiedType = FullType.unspecified,
  }) {
    return _serializeProperties(serializers, object, specifiedType: specifiedType).toList();
  }

  void _deserializeProperties(
    Serializers serializers,
    Object serialized, {
    FullType specifiedType = FullType.unspecified,
    required List<Object?> serializedList,
    required LessonResponseBuilder result,
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
        case r'position':
          final valueDes = serializers.deserialize(
            value,
            specifiedType: const FullType(int),
          ) as int;
          result.position = valueDes;
          break;
        case r'quiz':
          final valueDes = serializers.deserialize(
            value,
            specifiedType: const FullType.nullable(QuizResponse),
          ) as QuizResponse?;
          if (valueDes == null) continue;
          result.quiz.replace(valueDes);
          break;
        default:
          unhandled.add(key);
          unhandled.add(value);
          break;
      }
    }
  }

  @override
  LessonResponse deserialize(
    Serializers serializers,
    Object serialized, {
    FullType specifiedType = FullType.unspecified,
  }) {
    final result = LessonResponseBuilder();
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


