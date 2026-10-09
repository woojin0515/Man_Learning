// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'quiz_response.dart';

// **************************************************************************
// BuiltValueGenerator
// **************************************************************************

class _$QuizResponse extends QuizResponse {
  @override
  final String id;
  @override
  final BuiltList<QuestionResponse> questions;

  factory _$QuizResponse([void Function(QuizResponseBuilder)? updates]) =>
      (QuizResponseBuilder()..update(updates))._build();

  _$QuizResponse._({required this.id, required this.questions}) : super._();
  @override
  QuizResponse rebuild(void Function(QuizResponseBuilder) updates) =>
      (toBuilder()..update(updates)).build();

  @override
  QuizResponseBuilder toBuilder() => QuizResponseBuilder()..replace(this);

  @override
  bool operator ==(Object other) {
    if (identical(other, this)) return true;
    return other is QuizResponse &&
        id == other.id &&
        questions == other.questions;
  }

  @override
  int get hashCode {
    var _$hash = 0;
    _$hash = $jc(_$hash, id.hashCode);
    _$hash = $jc(_$hash, questions.hashCode);
    _$hash = $jf(_$hash);
    return _$hash;
  }

  @override
  String toString() {
    return (newBuiltValueToStringHelper(r'QuizResponse')
          ..add('id', id)
          ..add('questions', questions))
        .toString();
  }
}

class QuizResponseBuilder
    implements Builder<QuizResponse, QuizResponseBuilder> {
  _$QuizResponse? _$v;

  String? _id;
  String? get id => _$this._id;
  set id(String? id) => _$this._id = id;

  ListBuilder<QuestionResponse>? _questions;
  ListBuilder<QuestionResponse> get questions =>
      _$this._questions ??= ListBuilder<QuestionResponse>();
  set questions(ListBuilder<QuestionResponse>? questions) =>
      _$this._questions = questions;

  QuizResponseBuilder() {
    QuizResponse._defaults(this);
  }

  QuizResponseBuilder get _$this {
    final $v = _$v;
    if ($v != null) {
      _id = $v.id;
      _questions = $v.questions.toBuilder();
      _$v = null;
    }
    return this;
  }

  @override
  void replace(QuizResponse other) {
    _$v = other as _$QuizResponse;
  }

  @override
  void update(void Function(QuizResponseBuilder)? updates) {
    if (updates != null) updates(this);
  }

  @override
  QuizResponse build() => _build();

  _$QuizResponse _build() {
    _$QuizResponse _$result;
    try {
      _$result = _$v ??
          _$QuizResponse._(
            id: BuiltValueNullFieldError.checkNotNull(
                id, r'QuizResponse', 'id'),
            questions: questions.build(),
          );
    } catch (_) {
      late String _$failedField;
      try {
        _$failedField = 'questions';
        questions.build();
      } catch (e) {
        throw BuiltValueNestedFieldError(
            r'QuizResponse', _$failedField, e.toString());
      }
      rethrow;
    }
    replace(_$result);
    return _$result;
  }
}

// ignore_for_file: deprecated_member_use_from_same_package,type=lint
