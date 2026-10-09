/// Base URL configuration for talking to `ManLearning.Api`.
///
/// Kept as a single, explicit constant/override point rather than hard-coding the URL inside
/// [DioProvider] or the generated client, so a future production/Azure base URL (ADR-0010) is a
/// one-line change here, not a search-and-replace across the app.
class ApiConfig {
  const ApiConfig._();

  /// Local development default: `ManLearning.Api` run via
  /// `dotnet run --project src/ManLearning.Api --urls http://localhost:5299`
  /// (the same port used throughout `docs/spikes/openapi-dart-client-generation.md` and the API
  /// vertical slice's own tests).
  ///
  /// Overridable at build/run time with `--dart-define=API_BASE_URL=...`, e.g. for a future
  /// staging/production Azure App Service URL — no code change required once that URL exists.
  static const String baseUrl = String.fromEnvironment(
    'API_BASE_URL',
    defaultValue: 'http://localhost:5299',
  );
}
