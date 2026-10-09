// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'lesson_response.dart';

// **************************************************************************
// BuiltValueGenerator
// **************************************************************************

class _$LessonResponse extends LessonResponse {
  @override
  final String id;
  @override
  final String title;
  @override
  final int position;
  @override
  final QuizResponse? quiz;

  factory _$LessonResponse([void Function(LessonResponseBuilder)? updates]) =>
      (LessonResponseBuilder()..update(updates))._build();

  _$LessonResponse._(
      {required this.id,
      required this.title,
      required this.position,
      this.quiz})
      : super._();
  @override
  LessonResponse rebuild(void Function(LessonResponseBuilder) updates) =>
      (toBuilder()..update(updates)).build();

  @override
  LessonResponseBuilder toBuilder() => LessonResponseBuilder()..replace(this);

  @override
  bool operator ==(Object other) {
    if (identical(other, this)) return true;
    return other is LessonResponse &&
        id == other.id &&
        title == other.title &&
        position == other.position &&
        quiz == other.quiz;
  }

  @override
  int get hashCode {
    var _$hash = 0;
    _$hash = $jc(_$hash, id.hashCode);
    _$hash = $jc(_$hash, title.hashCode);
    _$hash = $jc(_$hash, position.hashCode);
    _$hash = $jc(_$hash, quiz.hashCode);
    _$hash = $jf(_$hash);
    return _$hash;
  }

  @override
  String toString() {
    return (newBuiltValueToStringHelper(r'LessonResponse')
          ..add('id', id)
          ..add('title', title)
          ..add('position', position)
          ..add('quiz', quiz))
        .toString();
  }
}

class LessonResponseBuilder
    implements Builder<LessonResponse, LessonResponseBuilder> {
  _$LessonResponse? _$v;

  String? _id;
  String? get id => _$this._id;
  set id(String? id) => _$this._id = id;

  String? _title;
  String? get title => _$this._title;
  set title(String? title) => _$this._title = title;

  int? _position;
  int? get position => _$this._position;
  set position(int? position) => _$this._position = position;

  QuizResponseBuilder? _quiz;
  QuizResponseBuilder get quiz => _$this._quiz ??= QuizResponseBuilder();
  set quiz(QuizResponseBuilder? quiz) => _$this._quiz = quiz;

  LessonResponseBuilder() {
    LessonResponse._defaults(this);
  }

  LessonResponseBuilder get _$this {
    final $v = _$v;
    if ($v != null) {
      _id = $v.id;
      _title = $v.title;
      _position = $v.position;
      _quiz = $v.quiz?.toBuilder();
      _$v = null;
    }
    return this;
  }

  @override
  void replace(LessonResponse other) {
    _$v = other as _$LessonResponse;
  }

  @override
  void update(void Function(LessonResponseBuilder)? updates) {
    if (updates != null) updates(this);
  }

  @override
  LessonResponse build() => _build();

  _$LessonResponse _build() {
    _$LessonResponse _$result;
    try {
      _$result = _$v ??
          _$LessonResponse._(
            id: BuiltValueNullFieldError.checkNotNull(
                id, r'LessonResponse', 'id'),
            title: BuiltValueNullFieldError.checkNotNull(
                title, r'LessonResponse', 'title'),
            position: BuiltValueNullFieldError.checkNotNull(
                position, r'LessonResponse', 'position'),
            quiz: _quiz?.build(),
          );
    } catch (_) {
      late String _$failedField;
      try {
        _$failedField = 'quiz';
        _quiz?.build();
      } catch (e) {
        throw BuiltValueNestedFieldError(
            r'LessonResponse', _$failedField, e.toString());
      }
      rethrow;
    }
    replace(_$result);
    return _$result;
  }
}

// ignore_for_file: deprecated_member_use_from_same_package,type=lint
