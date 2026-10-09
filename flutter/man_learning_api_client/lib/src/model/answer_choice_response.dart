//
// AUTO-GENERATED FILE, DO NOT MODIFY!
//

// ignore_for_file: unused_element
import 'package:built_value/built_value.dart';
import 'package:built_value/serializer.dart';

part 'answer_choice_response.g.dart';

/// AnswerChoiceResponse
///
/// Properties:
/// * [id] 
/// * [text] 
@BuiltValue()
abstract class AnswerChoiceResponse implements Built<AnswerChoiceResponse, AnswerChoiceResponseBuilder> {
  @BuiltValueField(wireName: r'id')
  String get id;

  @BuiltValueField(wireName: r'text')
  String get text;

  AnswerChoiceResponse._();

  factory AnswerChoiceResponse([void updates(AnswerChoiceResponseBuilder b)]) = _$AnswerChoiceResponse;

  @BuiltValueHook(initializeBuilder: true)
  static void _defaults(AnswerChoiceResponseBuilder b) => b;

  @BuiltValueSerializer(custom: true)
  static Serializer<AnswerChoiceResponse> get serializer => _$AnswerChoiceResponseSerializer();
}

class _$AnswerChoiceResponseSerializer implements PrimitiveSerializer<AnswerChoiceResponse> {
  @override
  final Iterable<Type> types = const [AnswerChoiceResponse, _$AnswerChoiceResponse];

  @override
  final String wireName = r'AnswerChoiceResponse';

  Iterable<Object?> _serializeProperties(
    Serializers serializers,
    AnswerChoiceResponse object, {
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
  }

  @override
  Object serialize(
    Serializers serializers,
    AnswerChoiceResponse object, {
    FullType specifiedType = FullType.unspecified,
  }) {
    return _serializeProperties(serializers, object, specifiedType: specifiedType).toList();
  }

  void _deserializeProperties(
    Serializers serializers,
    Object serialized, {
    FullType specifiedType = FullType.unspecified,
    required List<Object?> serializedList,
    required AnswerChoiceResponseBuilder result,
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
        default:
          unhandled.add(key);
          unhandled.add(value);
          break;
      }
    }
  }

  @override
  AnswerChoiceResponse deserialize(
    Serializers serializers,
    Object serialized, {
    FullType specifiedType = FullType.unspecified,
  }) {
    final result = AnswerChoiceResponseBuilder();
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


