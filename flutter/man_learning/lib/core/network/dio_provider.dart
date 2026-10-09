import 'package:dio/dio.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import 'api_config.dart';

/// The single [Dio] instance the whole app's HTTP layer is built on. Centralizing it here (rather
/// than letting each generated API client construct its own) means base URL, timeouts, and future
/// cross-cutting concerns (an auth interceptor once ADR-0007 lands, logging, retry) only need to
/// be configured in one place.
///
/// Intentionally minimal for this vertical slice: no auth interceptor, no token refresh, no retry
/// policy yet — only what `GET /api/courses` (an anonymous-access endpoint) needs.
final dioProvider = Provider<Dio>((ref) {
  return Dio(
    BaseOptions(
      baseUrl: ApiConfig.baseUrl,
      connectTimeout: const Duration(seconds: 5),
      receiveTimeout: const Duration(seconds: 10),
      headers: const {'Accept': 'application/json'},
    ),
  );
});
