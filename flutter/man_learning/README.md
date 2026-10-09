# man_learning

Flutter Web client for Man Learning. First vertical slice: Courses screen backed by
`GET /api/courses` on `ManLearning.Api`.

```text
CoursesScreen (Riverpod ConsumerWidget)
    ↓
coursesProvider (FutureProvider<List<Course>>)
    ↓
CoursesRepository (features/courses/data)
    ↓
CoursesApi (generated, man_learning_api_client package)
    ↓
Dio (core/network/dio_provider.dart)
    ↓
GET /api/courses → ManLearning.Api
```

## Project layout

```text
flutter/
├── man_learning/              # this Flutter app (feature-first: app/, core/, features/)
└── man_learning_api_client/   # generated OpenAPI client (sibling Dart package, see below)
```

`man_learning_api_client` is a *separate* Dart package, not a folder inside `man_learning/lib/`.
OpenAPI Generator's `dart-dio` target produces a complete, self-contained Dart package (its own
`pubspec.yaml`/`build.yaml`), which cannot be nested inside another package's `lib/`. `man_learning`
depends on it via a local path dependency in `pubspec.yaml`:
`man_learning_api_client: {path: ../man_learning_api_client}`.

## Generated client policy

The generated client (`flutter/man_learning_api_client/`) is **not committed** to source control.
It is treated as a reproducible build artifact: the OpenAPI spec + a pinned generator version
(`tool/generate-dart-client.sh`) are the source of truth, and the generated package is regenerated
locally (or in CI, once CI exists) before building/testing the app.

This was a deliberate deviation from the earlier spike recommendation ("commit nothing generated")
once it reached an actual Flutter app: a `flutter pub get`/`flutter run`/`flutter test` cycle needs
`man_learning_api_client` to already exist on disk as a real package. The resolution is **not** to
commit it, but to always regenerate it first — see "Local development" below. Rationale:

* **CI reproducibility** — generation is scripted and pinned to generator 7.25.0, so any machine
  (or CI runner) produces byte-identical output from the same OpenAPI spec.
* **Diff noise** — generated `built_value` `.g.dart` files are large and churn on every schema
  change; keeping them out of the repo keeps real diffs reviewable.
* **Generator version lock is already explicit** — `tool/generate-dart-client.sh` pins
  `OPENAPI_GENERATOR_VERSION=7.25.0`, so regeneration is deterministic without needing the output
  committed as a fallback.
* **Contributor experience** — the one extra step (`./tool/generate-dart-client.sh`) is documented
  and scripted, so it doesn't meaningfully slow down onboarding.

## Local development

1. Run the backend API (from the repo root):
   ```bash
   dotnet run --project src/ManLearning.Api/ManLearning.Api.csproj --urls http://localhost:5299
   ```
2. Generate the Dart API client (from the repo root, with the API from step 1 already running):
   ```bash
   ./tool/generate-dart-client.sh
   ```
3. Fetch Flutter dependencies and run the app:
   ```bash
   cd flutter/man_learning
   flutter pub get
   flutter run -d chrome --web-port=5500
   ```
   Port `5500` matches the Flutter Web dev origin ASP.NET Core's development-only CORS policy
   allows (`FlutterWeb:DevOrigins` in `src/ManLearning.Api/appsettings.Development.json`). Using a
   different port will fail CORS unless that origin is added to the same setting.

### API base URL

`lib/core/network/api_config.dart` defaults to `http://localhost:5299`. Override it without code
changes via:

```bash
flutter run -d chrome --web-port=5500 --dart-define=API_BASE_URL=http://localhost:5299
```

A future Azure deployment would pass its own `API_BASE_URL` the same way; this slice does not
configure or implement that deployment.

## Testing

```bash
flutter analyze
flutter test
flutter build web
```

Tests fake only the HTTP transport (`test/features/courses/fixture_http_client_adapter.dart`, a
minimal `HttpClientAdapter`), not the generated client or its `built_value` (de)serialization — so
`CoursesRepository`/`coursesProvider`/`CoursesScreen` tests exercise the real generated `CoursesApi`
code path end-to-end, just without a live network call.

Riverpod 3's automatic retry (up to 10 attempts with exponential backoff on provider errors) is
disabled in these tests (`ProviderContainer(retry: (_, __) => null)` /
`ProviderScope(retry: ...)`) so failure-path tests settle immediately instead of retrying for
several seconds. The app itself keeps the Riverpod default.

## Dependencies

| Package                | Purpose                                             |
| ----------------------- | ---------------------------------------------------- |
| flutter_riverpod        | State management + dependency injection              |
| dio                     | HTTP client                                           |
| go_router               | Navigation (`/` → `/courses`)                        |
| man_learning_api_client | Generated OpenAPI client (dart-dio + built_value)     |
| built_value / built_collection / one_of | Required by the generated client's models |

Not yet implemented in this slice: authentication, Lessons/Quiz/XP features, Azure deployment.
