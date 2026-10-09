// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'lesson_summary_response.dart';

// **************************************************************************
// BuiltValueGenerator
// **************************************************************************

class _$LessonSummaryResponse extends LessonSummaryResponse {
  @override
  final String id;
  @override
  final String title;
  @override
  final int position;
  @override
  final bool hasQuiz;

  factory _$LessonSummaryResponse(
          [void Function(LessonSummaryResponseBuilder)? updates]) =>
      (LessonSummaryResponseBuilder()..update(updates))._build();

  _$LessonSummaryResponse._(
      {required this.id,
      required this.title,
      required this.position,
      required this.hasQuiz})
      : super._();
  @override
  LessonSummaryResponse rebuild(
          void Function(LessonSummaryResponseBuilder) updates) =>
      (toBuilder()..update(updates)).build();

  @override
  LessonSummaryResponseBuilder toBuilder() =>
      LessonSummaryResponseBuilder()..replace(this);

  @override
  bool operator ==(Object other) {
    if (identical(other, this)) return true;
    return other is LessonSummaryResponse &&
        id == other.id &&
        title == other.title &&
        position == other.position &&
        hasQuiz == other.hasQuiz;
  }

  @override
  int get hashCode {
    var _$hash = 0;
    _$hash = $jc(_$hash, id.hashCode);
    _$hash = $jc(_$hash, title.hashCode);
    _$hash = $jc(_$hash, position.hashCode);
    _$hash = $jc(_$hash, hasQuiz.hashCode);
    _$hash = $jf(_$hash);
    return _$hash;
  }

  @override
  String toString() {
    return (newBuiltValueToStringHelper(r'LessonSummaryResponse')
          ..add('id', id)
          ..add('title', title)
          ..add('position', position)
          ..add('hasQuiz', hasQuiz))
        .toString();
  }
}

class LessonSummaryResponseBuilder
    implements Builder<LessonSummaryResponse, LessonSummaryResponseBuilder> {
  _$LessonSummaryResponse? _$v;

  String? _id;
  String? get id => _$this._id;
  set id(String? id) => _$this._id = id;

  String? _title;
  String? get title => _$this._title;
  set title(String? title) => _$this._title = title;

  int? _position;
  int? get position => _$this._position;
  set position(int? position) => _$this._position = position;

  bool? _hasQuiz;
  bool? get hasQuiz => _$this._hasQuiz;
  set hasQuiz(bool? hasQuiz) => _$this._hasQuiz = hasQuiz;

  LessonSummaryResponseBuilder() {
    LessonSummaryResponse._defaults(this);
  }

  LessonSummaryResponseBuilder get _$this {
    final $v = _$v;
    if ($v != null) {
      _id = $v.id;
      _title = $v.title;
      _position = $v.position;
      _hasQuiz = $v.hasQuiz;
      _$v = null;
    }
    return this;
  }

  @override
  void replace(LessonSummaryResponse other) {
    _$v = other as _$LessonSummaryResponse;
  }

  @override
  void update(void Function(LessonSummaryResponseBuilder)? updates) {
    if (updates != null) updates(this);
  }

  @override
  LessonSummaryResponse build() => _build();

  _$LessonSummaryResponse _build() {
    final _$result = _$v ??
        _$LessonSummaryResponse._(
          id: BuiltValueNullFieldError.checkNotNull(
              id, r'LessonSummaryResponse', 'id'),
          title: BuiltValueNullFieldError.checkNotNull(
              title, r'LessonSummaryResponse', 'title'),
          position: BuiltValueNullFieldError.checkNotNull(
              position, r'LessonSummaryResponse', 'position'),
          hasQuiz: BuiltValueNullFieldError.checkNotNull(
              hasQuiz, r'LessonSummaryResponse', 'hasQuiz'),
        );
    replace(_$result);
    return _$result;
  }
}

// ignore_for_file: deprecated_member_use_from_same_package,type=lint
