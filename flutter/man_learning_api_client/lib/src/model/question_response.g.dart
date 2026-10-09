// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'question_response.dart';

// **************************************************************************
// BuiltValueGenerator
// **************************************************************************

class _$QuestionResponse extends QuestionResponse {
  @override
  final String id;
  @override
  final String text;
  @override
  final BuiltList<AnswerChoiceResponse> answerChoices;

  factory _$QuestionResponse(
          [void Function(QuestionResponseBuilder)? updates]) =>
      (QuestionResponseBuilder()..update(updates))._build();

  _$QuestionResponse._(
      {required this.id, required this.text, required this.answerChoices})
      : super._();
  @override
  QuestionResponse rebuild(void Function(QuestionResponseBuilder) updates) =>
      (toBuilder()..update(updates)).build();

  @override
  QuestionResponseBuilder toBuilder() =>
      QuestionResponseBuilder()..replace(this);

  @override
  bool operator ==(Object other) {
    if (identical(other, this)) return true;
    return other is QuestionResponse &&
        id == other.id &&
        text == other.text &&
        answerChoices == other.answerChoices;
  }

  @override
  int get hashCode {
    var _$hash = 0;
    _$hash = $jc(_$hash, id.hashCode);
    _$hash = $jc(_$hash, text.hashCode);
    _$hash = $jc(_$hash, answerChoices.hashCode);
    _$hash = $jf(_$hash);
    return _$hash;
  }

  @override
  String toString() {
    return (newBuiltValueToStringHelper(r'QuestionResponse')
          ..add('id', id)
          ..add('text', text)
          ..add('answerChoices', answerChoices))
        .toString();
  }
}

class QuestionResponseBuilder
    implements Builder<QuestionResponse, QuestionResponseBuilder> {
  _$QuestionResponse? _$v;

  String? _id;
  String? get id => _$this._id;
  set id(String? id) => _$this._id = id;

  String? _text;
  String? get text => _$this._text;
  set text(String? text) => _$this._text = text;

  ListBuilder<AnswerChoiceResponse>? _answerChoices;
  ListBuilder<AnswerChoiceResponse> get answerChoices =>
      _$this._answerChoices ??= ListBuilder<AnswerChoiceResponse>();
  set answerChoices(ListBuilder<AnswerChoiceResponse>? answerChoices) =>
      _$this._answerChoices = answerChoices;

  QuestionResponseBuilder() {
    QuestionResponse._defaults(this);
  }

  QuestionResponseBuilder get _$this {
    final $v = _$v;
    if ($v != null) {
      _id = $v.id;
      _text = $v.text;
      _answerChoices = $v.answerChoices.toBuilder();
      _$v = null;
    }
    return this;
  }

  @override
  void replace(QuestionResponse other) {
    _$v = other as _$QuestionResponse;
  }

  @override
  void update(void Function(QuestionResponseBuilder)? updates) {
    if (updates != null) updates(this);
  }

  @override
  QuestionResponse build() => _build();

  _$QuestionResponse _build() {
    _$QuestionResponse _$result;
    try {
      _$result = _$v ??
          _$QuestionResponse._(
            id: BuiltValueNullFieldError.checkNotNull(
                id, r'QuestionResponse', 'id'),
            text: BuiltValueNullFieldError.checkNotNull(
                text, r'QuestionResponse', 'text'),
            answerChoices: answerChoices.build(),
          );
    } catch (_) {
      late String _$failedField;
      try {
        _$failedField = 'answerChoices';
        answerChoices.build();
      } catch (e) {
        throw BuiltValueNestedFieldError(
            r'QuestionResponse', _$failedField, e.toString());
      }
      rethrow;
    }
    replace(_$result);
    return _$result;
  }
}

// ignore_for_file: deprecated_member_use_from_same_package,type=lint
