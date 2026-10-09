//
// AUTO-GENERATED FILE, DO NOT MODIFY!
//

// ignore_for_file: unused_element
import 'package:built_collection/built_collection.dart';
import 'package:man_learning_api_client/src/model/answer_choice_response.dart';
import 'package:built_value/built_value.dart';
import 'package:built_value/serializer.dart';

part 'question_response.g.dart';

/// QuestionResponse
///
/// Properties:
/// * [id] 
/// * [text] 
/// * [answerChoices] 
@BuiltValue()
abstract class QuestionResponse implements Built<QuestionResponse, QuestionResponseBuilder> {
  @BuiltValueField(wireName: r'id')
  String get id;

  @BuiltValueField(wireName: r'text')
  String get text;

  @BuiltValueField(wireName: r'answerChoices')
  BuiltList<AnswerChoiceResponse> get answerChoices;

  QuestionResponse._();

  factory QuestionResponse([void updates(QuestionResponseBuilder b)]) = _$QuestionResponse;

  @BuiltValueHook(initializeBuilder: true)
  static void _defaults(QuestionResponseBuilder b) => b;

  @BuiltValueSerializer(custom: true)
  static Serializer<QuestionResponse> get serializer => _$QuestionResponseSerializer();
}

class _$QuestionResponseSerializer implements PrimitiveSerializer<QuestionResponse> {
  @override
  final Iterable<Type> types = const [QuestionResponse, _$QuestionResponse];

  @override
  final String wireName = r'QuestionResponse';

  Iterable<Object?> _serializeProperties(
    Serializers serializers,
    QuestionResponse object, {
    FullType specifiedType = FullType.unspecified,
  }) sync* {
    yield r'id';
    yield serializers.serialize(
      object.id,
      specifiedType: const FullType(String),
    );
    yield r'text';
    yield serializers.serialize(
      object.text,
      specifiedType: const FullType(String),
    );
    yield r'answerChoices';
    yield serializers.serialize(
      object.answerChoices,
      specifiedType: const FullType(BuiltList, [FullType(AnswerChoiceResponse)]),
    );
  }

  @override
  Object serialize(
    Serializers serializers,
    QuestionResponse object, {
    FullType specifiedType = FullType.unspecified,
  }) {
    return _serializeProperties(serializers, object, specifiedType: specifiedType).toList();
  }

  void _deserializeProperties(
    Serializers serializers,
    Object serialized, {
    FullType specifiedType = FullType.unspecified,
    required List<Object?> serializedList,
    required QuestionResponseBuilder result,
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
        case r'text':
          final valueDes = serializers.deserialize(
            value,
            specifiedType: const FullType(String),
          ) as String;
          result.text = valueDes;
          break;
        case r'answerChoices':
          final valueDes = serializers.deserialize(
            value,
            specifiedType: const FullType(BuiltList, [FullType(AnswerChoiceResponse)]),
          ) as BuiltList<AnswerChoiceResponse>;
          result.answerChoices.replace(valueDes);
          break;
        default:
          unhandled.add(key);
          unhandled.add(value);
          break;
      }
    }
  }

  @override
  QuestionResponse deserialize(
    Serializers serializers,
    Object serialized, {
    FullType specifiedType = FullType.unspecified,
  }) {
    final result = QuestionResponseBuilder();
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


