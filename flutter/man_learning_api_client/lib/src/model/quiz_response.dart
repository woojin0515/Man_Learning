//
// AUTO-GENERATED FILE, DO NOT MODIFY!
//

// ignore_for_file: unused_element
import 'package:built_collection/built_collection.dart';
import 'package:man_learning_api_client/src/model/question_response.dart';
import 'package:built_value/built_value.dart';
import 'package:built_value/serializer.dart';

part 'quiz_response.g.dart';

/// QuizResponse
///
/// Properties:
/// * [id] 
/// * [questions] 
@BuiltValue()
abstract class QuizResponse implements Built<QuizResponse, QuizResponseBuilder> {
  @BuiltValueField(wireName: r'id')
  String get id;

  @BuiltValueField(wireName: r'questions')
  BuiltList<QuestionResponse> get questions;

  QuizResponse._();

  factory QuizResponse([void updates(QuizResponseBuilder b)]) = _$QuizResponse;

  @BuiltValueHook(initializeBuilder: true)
  static void _defaults(QuizResponseBuilder b) => b;

  @BuiltValueSerializer(custom: true)
  static Serializer<QuizResponse> get serializer => _$QuizResponseSerializer();
}

class _$QuizResponseSerializer implements PrimitiveSerializer<QuizResponse> {
  @override
  final Iterable<Type> types = const [QuizResponse, _$QuizResponse];

  @override
  final String wireName = r'QuizResponse';

  Iterable<Object?> _serializeProperties(
    Serializers serializers,
    QuizResponse object, {
    FullType specifiedType = FullType.unspecified,
  }) sync* {
    yield r'id';
    yield serializers.serialize(
      object.id,
      specifiedType: const FullType(String),
    );
    yield r'questions';
    yield serializers.serialize(
      object.questions,
      specifiedType: const FullType(BuiltList, [FullType(QuestionResponse)]),
    );
  }

  @override
  Object serialize(
    Serializers serializers,
    QuizResponse object, {
    FullType specifiedType = FullType.unspecified,
  }) {
    return _serializeProperties(serializers, object, specifiedType: specifiedType).toList();
  }

  void _deserializeProperties(
    Serializers serializers,
    Object serialized, {
    FullType specifiedType = FullType.unspecified,
    required List<Object?> serializedList,
    required QuizResponseBuilder result,
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
        case r'questions':
          final valueDes = serializers.deserialize(
            value,
            specifiedType: const FullType(BuiltList, [FullType(QuestionResponse)]),
          ) as BuiltList<QuestionResponse>;
          result.questions.replace(valueDes);
          break;
        default:
          unhandled.add(key);
          unhandled.add(value);
          break;
      }
    }
  }

  @override
  QuizResponse deserialize(
    Serializers serializers,
    Object serialized, {
    FullType specifiedType = FullType.unspecified,
  }) {
    final result = QuizResponseBuilder();
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


