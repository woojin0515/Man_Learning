import 'dart:async';
import 'dart:typed_data';

import 'package:dio/dio.dart';

/// A minimal fake [HttpClientAdapter] for tests: instead of hitting the network, it returns a
/// canned response for every request. This lets [CoursesRepository] and related tests exercise
/// the *real* generated [CoursesApi]/built_value deserialization path end-to-end, without
/// actually running `ManLearning.Api` — only the transport layer is faked, not the API client or
/// its (de)serialization.
class FixtureHttpClientAdapter implements HttpClientAdapter {
  FixtureHttpClientAdapter({required this.statusCode, required this.body});

  final int statusCode;
  final String body;

  @override
  Future<ResponseBody> fetch(
    RequestOptions options,
    Stream<Uint8List>? requestStream,
    Future<void>? cancelFuture,
  ) async {
    return ResponseBody.fromString(
      body,
      statusCode,
      headers: {
        Headers.contentTypeHeader: [Headers.jsonContentType],
      },
    );
  }

  @override
  void close({bool force = false}) {}
}
