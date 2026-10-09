// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'course_response.dart';

// **************************************************************************
// BuiltValueGenerator
// **************************************************************************

class _$CourseResponse extends CourseResponse {
  @override
  final String id;
  @override
  final String title;
  @override
  final int lessonCount;

  factory _$CourseResponse([void Function(CourseResponseBuilder)? updates]) =>
      (CourseResponseBuilder()..update(updates))._build();

  _$CourseResponse._(
      {required this.id, required this.title, required this.lessonCount})
      : super._();
  @override
  CourseResponse rebuild(void Function(CourseResponseBuilder) updates) =>
      (toBuilder()..update(updates)).build();

  @override
  CourseResponseBuilder toBuilder() => CourseResponseBuilder()..replace(this);

  @override
  bool operator ==(Object other) {
    if (identical(other, this)) return true;
    return other is CourseResponse &&
        id == other.id &&
        title == other.title &&
        lessonCount == other.lessonCount;
  }

  @override
  int get hashCode {
    var _$hash = 0;
    _$hash = $jc(_$hash, id.hashCode);
    _$hash = $jc(_$hash, title.hashCode);
    _$hash = $jc(_$hash, lessonCount.hashCode);
    _$hash = $jf(_$hash);
    return _$hash;
  }

  @override
  String toString() {
    return (newBuiltValueToStringHelper(r'CourseResponse')
          ..add('id', id)
          ..add('title', title)
          ..add('lessonCount', lessonCount))
        .toString();
  }
}

class CourseResponseBuilder
    implements Builder<CourseResponse, CourseResponseBuilder> {
  _$CourseResponse? _$v;

  String? _id;
  String? get id => _$this._id;
  set id(String? id) => _$this._id = id;

  String? _title;
  String? get title => _$this._title;
  set title(String? title) => _$this._title = title;

  int? _lessonCount;
  int? get lessonCount => _$this._lessonCount;
  set lessonCount(int? lessonCount) => _$this._lessonCount = lessonCount;

  CourseResponseBuilder() {
    CourseResponse._defaults(this);
  }

  CourseResponseBuilder get _$this {
    final $v = _$v;
    if ($v != null) {
      _id = $v.id;
      _title = $v.title;
      _lessonCount = $v.lessonCount;
      _$v = null;
    }
    return this;
  }

  @override
  void replace(CourseResponse other) {
    _$v = other as _$CourseResponse;
  }

  @override
  void update(void Function(CourseResponseBuilder)? updates) {
    if (updates != null) updates(this);
  }

  @override
  CourseResponse build() => _build();

  _$CourseResponse _build() {
    final _$result = _$v ??
        _$CourseResponse._(
          id: BuiltValueNullFieldError.checkNotNull(
              id, r'CourseResponse', 'id'),
          title: BuiltValueNullFieldError.checkNotNull(
              title, r'CourseResponse', 'title'),
          lessonCount: BuiltValueNullFieldError.checkNotNull(
              lessonCount, r'CourseResponse', 'lessonCount'),
        );
    replace(_$result);
    return _$result;
  }
}

// ignore_for_file: deprecated_member_use_from_same_package,type=lint
