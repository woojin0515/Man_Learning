// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'answer_choice_response.dart';

// **************************************************************************
// BuiltValueGenerator
// **************************************************************************

class _$AnswerChoiceResponse extends AnswerChoiceResponse {
  @override
  final String id;
  @override
  final String text;

  factory _$AnswerChoiceResponse(
          [void Function(AnswerChoiceResponseBuilder)? updates]) =>
      (AnswerChoiceResponseBuilder()..update(updates))._build();

  _$AnswerChoiceResponse._({required this.id, required this.text}) : super._();
  @override
  AnswerChoiceResponse rebuild(
          void Function(AnswerChoiceResponseBuilder) updates) =>
      (toBuilder()..update(updates)).build();

  @override
  AnswerChoiceResponseBuilder toBuilder() =>
      AnswerChoiceResponseBuilder()..replace(this);

  @override
  bool operator ==(Object other) {
    if (identical(other, this)) return true;
    return other is AnswerChoiceResponse &&
        id == other.id &&
        text == other.text;
  }

  @override
  int get hashCode {
    var _$hash = 0;
    _$hash = $jc(_$hash, id.hashCode);
    _$hash = $jc(_$hash, text.hashCode);
    _$hash = $jf(_$hash);
    return _$hash;
  }

  @override
  String toString() {
    return (newBuiltValueToStringHelper(r'AnswerChoiceResponse')
          ..add('id', id)
          ..add('text', text))
        .toString();
  }
}

class AnswerChoiceResponseBuilder
    implements Builder<AnswerChoiceResponse, AnswerChoiceResponseBuilder> {
  _$AnswerChoiceResponse? _$v;

  String? _id;
  String? get id => _$this._id;
  set id(String? id) => _$this._id = id;

  String? _text;
  String? get text => _$this._text;
  set text(String? text) => _$this._text = text;

  AnswerChoiceResponseBuilder() {
    AnswerChoiceResponse._defaults(this);
  }

  AnswerChoiceResponseBuilder get _$this {
    final $v = _$v;
    if ($v != null) {
      _id = $v.id;
      _text = $v.text;
      _$v = null;
    }
    return this;
  }

  @override
  void replace(AnswerChoiceResponse other) {
    _$v = other as _$AnswerChoiceResponse;
  }

  @override
  void update(void Function(AnswerChoiceResponseBuilder)? updates) {
    if (updates != null) updates(this);
  }

  @override
  AnswerChoiceResponse build() => _build();

  _$AnswerChoiceResponse _build() {
    final _$result = _$v ??
        _$AnswerChoiceResponse._(
          id: BuiltValueNullFieldError.checkNotNull(
              id, r'AnswerChoiceResponse', 'id'),
          text: BuiltValueNullFieldError.checkNotNull(
              text, r'AnswerChoiceResponse', 'text'),
        );
    replace(_$result);
    return _$result;
  }
}

// ignore_for_file: deprecated_member_use_from_same_package,type=lint
