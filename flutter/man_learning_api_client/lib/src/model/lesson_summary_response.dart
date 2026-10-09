//
// AUTO-GENERATED FILE, DO NOT MODIFY!
//

// ignore_for_file: unused_element
import 'package:built_value/built_value.dart';
import 'package:built_value/serializer.dart';

part 'lesson_summary_response.g.dart';

/// LessonSummaryResponse
///
/// Properties:
/// * [id] 
/// * [title] 
/// * [position] 
/// * [hasQuiz] 
@BuiltValue()
abstract class LessonSummaryResponse implements Built<LessonSummaryResponse, LessonSummaryResponseBuilder> {
  @BuiltValueField(wireName: r'id')
  String get id;

  @BuiltValueField(wireName: r'title')
  String get title;

  @BuiltValueField(wireName: r'position')
  int get position;

  @BuiltValueField(wireName: r'hasQuiz')
  bool get hasQuiz;

  LessonSummaryResponse._();

  factory LessonSummaryResponse([void updates(LessonSummaryResponseBuilder b)]) = _$LessonSummaryResponse;

  @BuiltValueHook(initializeBuilder: true)
  static void _defaults(LessonSummaryResponseBuilder b) => b;

  @BuiltValueSerializer(custom: true)
  static Serializer<LessonSummaryResponse> get serializer => _$LessonSummaryResponseSerializer();
}

class _$LessonSummaryResponseSerializer implements PrimitiveSerializer<LessonSummaryResponse> {
  @override
  final Iterable<Type> types = const [LessonSummaryResponse, _$LessonSummaryResponse];

  @override
  final String wireName = r'LessonSummaryResponse';

  Iterable<Object?> _serializeProperties(
    Serializers serializers,
    LessonSummaryResponse object, {
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
    yield r'hasQuiz';
    yield serializers.serialize(
      object.hasQuiz,
      specifiedType: const FullType(bool),
    );
  }

  @override
  Object serialize(
    Serializers serializers,
    LessonSummaryResponse object, {
    FullType specifiedType = FullType.unspecified,
  }) {
    return _serializeProperties(serializers, object, specifiedType: specifiedType).toList();
  }

  void _deserializeProperties(
    Serializers serializers,
    Object serialized, {
    FullType specifiedType = FullType.unspecified,
    required List<Object?> serializedList,
    required LessonSummaryResponseBuilder result,
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
        case r'hasQuiz':
          final valueDes = serializers.deserialize(
            value,
            specifiedType: const FullType(bool),
          ) as bool;
          result.hasQuiz = valueDes;
          break;
        default:
          unhandled.add(key);
          unhandled.add(value);
          break;
      }
    }
  }

  @override
  LessonSummaryResponse deserialize(
    Serializers serializers,
    Object serialized, {
    FullType specifiedType = FullType.unspecified,
  }) {
    final result = LessonSummaryResponseBuilder();
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


