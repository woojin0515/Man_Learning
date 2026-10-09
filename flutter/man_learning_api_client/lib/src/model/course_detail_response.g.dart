// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'course_detail_response.dart';

// **************************************************************************
// BuiltValueGenerator
// **************************************************************************

class _$CourseDetailResponse extends CourseDetailResponse {
  @override
  final String id;
  @override
  final String title;
  @override
  final BuiltList<LessonSummaryResponse> lessons;

  factory _$CourseDetailResponse(
          [void Function(CourseDetailResponseBuilder)? updates]) =>
      (CourseDetailResponseBuilder()..update(updates))._build();

  _$CourseDetailResponse._(
      {required this.id, required this.title, required this.lessons})
      : super._();
  @override
  CourseDetailResponse rebuild(
          void Function(CourseDetailResponseBuilder) updates) =>
      (toBuilder()..update(updates)).build();

  @override
  CourseDetailResponseBuilder toBuilder() =>
      CourseDetailResponseBuilder()..replace(this);

  @override
  bool operator ==(Object other) {
    if (identical(other, this)) return true;
    return other is CourseDetailResponse &&
        id == other.id &&
        title == other.title &&
        lessons == other.lessons;
  }

  @override
  int get hashCode {
    var _$hash = 0;
    _$hash = $jc(_$hash, id.hashCode);
    _$hash = $jc(_$hash, title.hashCode);
    _$hash = $jc(_$hash, lessons.hashCode);
    _$hash = $jf(_$hash);
    return _$hash;
  }

  @override
  String toString() {
    return (newBuiltValueToStringHelper(r'CourseDetailResponse')
          ..add('id', id)
          ..add('title', title)
          ..add('lessons', lessons))
        .toString();
  }
}

class CourseDetailResponseBuilder
    implements Builder<CourseDetailResponse, CourseDetailResponseBuilder> {
  _$CourseDetailResponse? _$v;

  String? _id;
  String? get id => _$this._id;
  set id(String? id) => _$this._id = id;

  String? _title;
  String? get title => _$this._title;
  set title(String? title) => _$this._title = title;

  ListBuilder<LessonSummaryResponse>? _lessons;
  ListBuilder<LessonSummaryResponse> get lessons =>
      _$this._lessons ??= ListBuilder<LessonSummaryResponse>();
  set lessons(ListBuilder<LessonSummaryResponse>? lessons) =>
      _$this._lessons = lessons;

  CourseDetailResponseBuilder() {
    CourseDetailResponse._defaults(this);
  }

  CourseDetailResponseBuilder get _$this {
    final $v = _$v;
    if ($v != null) {
      _id = $v.id;
      _title = $v.title;
      _lessons = $v.lessons.toBuilder();
      _$v = null;
    }
    return this;
  }

  @override
  void replace(CourseDetailResponse other) {
    _$v = other as _$CourseDetailResponse;
  }

  @override
  void update(void Function(CourseDetailResponseBuilder)? updates) {
    if (updates != null) updates(this);
  }

  @override
  CourseDetailResponse build() => _build();

  _$CourseDetailResponse _build() {
    _$CourseDetailResponse _$result;
    try {
      _$result = _$v ??
          _$CourseDetailResponse._(
            id: BuiltValueNullFieldError.checkNotNull(
                id, r'CourseDetailResponse', 'id'),
            title: BuiltValueNullFieldError.checkNotNull(
                title, r'CourseDetailResponse', 'title'),
            lessons: lessons.build(),
          );
    } catch (_) {
      late String _$failedField;
      try {
        _$failedField = 'lessons';
        lessons.build();
      } catch (e) {
        throw BuiltValueNestedFieldError(
            r'CourseDetailResponse', _$failedField, e.toString());
      }
      rethrow;
    }
    replace(_$result);
    return _$result;
  }
}

// ignore_for_file: deprecated_member_use_from_same_package,type=lint
