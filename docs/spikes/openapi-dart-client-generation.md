# Spike: OpenAPI → Dart Client Generation

## Trigger

ADR-0008 (OpenAPI as API Contract) deliberately deferred the choice of an OpenAPI-to-Dart
generator: "no Dart client/model generator is selected yet... before committing to one, a short
spike should generate Dart client code from the real `GET /api/courses` OpenAPI document and
evaluate it." The first API vertical slice (`ManLearning.Api`, `GET /api/courses`,
`CourseResponse`) is now implemented and producing a real OpenAPI document, which is the
precondition this spike needed before it could run against something other than a hand-written
spec.

This spike does **not** build a Flutter app. It only answers: *if we generate a Dart client from
the current, real `GET /api/courses` contract, is the output good enough to plug into the planned
Flutter + Dio (ADR-0009/0010) + Riverpod architecture?*

## Follow-up: `int32 → anyOf[integer,string]` resolved

The blocking issue originally found in this spike (Problems #1 below, and the "FAIL" row in the
integration assessment table) has since been **resolved**. See "Resolution" under Problems #1 for
the root cause, the fix, and before/after verification. `CourseResponse.lessonCount` now generates
as a plain Dart `int` with no wrapper type, and a regression test
(`tests/ManLearning.Api.Tests/OpenApiSchemaTests.cs`) now protects this generically for any
int32-formatted property in the OpenAPI document, not just `lessonCount`. The rest of this
document is left as originally written (including the "FAIL" table row, now superseded) so the
investigation trail stays intact; read the Resolution subsection for the current state.

## Scope and constraints

- No Flutter project was created (no `flutter create`, no `lib/main.dart`, no UI).
- No Riverpod/go_router code was written; integration was assessed by inspecting generated code,
  not by wiring it into a real app.
- No authentication was implemented; only whether the generated client has a plumbing point for a
  future `Authorization: Bearer <token>` header was checked.
- The real `GET /api/courses` / `CourseResponse` contract was not changed to make generation look
  better. One transient, local-only experiment is called out explicitly in "Problems" below and
  was reverted immediately (confirmed via `git diff` showing no changes to `Program.cs`).
- Nothing under `src/`, `tests/`, or `docs/decisions/` changed. The only new content is this file
  and `docs/spikes/openapi-dart-client-generation/` (captured OpenAPI document + a trimmed sample
  of generated code, kept as evidence, not as a dependency of the build).

## Input

- **Source API**: `ManLearning.Api`, run locally (`dotnet run --no-build --urls
  http://localhost:5299`), OpenAPI document captured from the live `/openapi/v1.json` endpoint
  (ASP.NET Core's built-in `Microsoft.AspNetCore.OpenApi` package — no Swashbuckle or other
  package is installed, matching ADR-0008 and the existing `ManLearning.Api.csproj`).
- **OpenAPI version produced**: `3.1.1` (the ASP.NET Core 10 default). A capture of this document
  is saved at `docs/spikes/openapi-dart-client-generation/openapi/openapi.json`.
- **Endpoint under test**: `GET /api/Courses` → `200` → `CourseResponse[]`.
- **Schema under test**: `CourseResponse { id: string(uuid), title: string, lessonCount: int32 }`.
  No nullable fields, no enums, no documented error schema exist in the current contract — this
  matches the vertical slice as built and is not something this spike added.

## Generator candidates considered

| Candidate | Dart support | OpenAPI 3.x | Dio | Null safety | Maintenance | Fit for this spike |
| --- | --- | --- | --- | --- | --- | --- |
| **OpenAPI Generator CLI — `dart-dio` generator** (`@openapitools/openapi-generator-cli`) | Yes, "stable" generator per its own output | 3.0 full, 3.1 "beta" (explicit warning at generation time) | Native — generated client takes a `Dio` instance directly | Yes (generated code is null-safe Dart) | Actively maintained (OpenAPI Generator project), used widely across many languages, not Dart-specific | **Selected for this spike** — runs standalone via Java, does not require a pre-existing Dart/Flutter project, which fits "don't create a Flutter project yet" |
| **OpenAPI Generator CLI — `dart` generator** (non-Dio, `http`-based) | Yes | Same engine as above | No — uses `package:http` | Yes | Same project/maintenance as above | Considered and rejected: ADR-0009 already plans a Dio-based Flutter client stack, so a generator whose output does not use Dio is a worse fit without adding any real benefit |
| **`openapi_generator` (Dart pub package, `gibahjoe/openapi-generator-dart`)** | Yes, Dio-based output by default | Wraps the same underlying OpenAPI Generator engine | Yes | Yes | Actively maintained pub package | Considered, not run in this spike: it works via `build_runner` and `@Openapi()` annotations *inside* a Dart/Flutter package (pubspec.yaml, `build_runner` dev-dependency). Standing one up would require creating a Dart project skeleton, which this spike is explicitly told not to do yet. It is the natural **second-phase** choice once a real Flutter project exists, since it integrates generation into the normal Dart build pipeline instead of a separate out-of-band CLI call. |

Two candidates were run end-to-end (same engine, two serialization-library configurations), which
satisfies the "max 2–3 candidates, at least one real generation" guidance without over-spending
time on redundant engines that all shell out to the same OpenAPI Generator core.

## Generated output

Command used (Java-based CLI, run via `npx`, no global install):

```bash
npx @openapitools/openapi-generator-cli generate \
  -i openapi.json \
  -g dart-dio \
  -o <out-dir> \
  --additional-properties='pubName=man_learning_api_client'
```

- **Generator**: `@openapitools/openapi-generator-cli` **7.25.0** (downloads the matching
  `openapi-generator-cli-7.25.0.jar`; requires a JVM — `openjdk 26.0.2` was already available
  locally via Homebrew. No Dart/Flutter SDK was required to *generate* the code, only to compile
  or run it, which this spike does not do).
- **Target generator**: `dart-dio`, reported as "considered stable" by the tool itself, with an
  explicit warning that "OpenAPI 3.1 support is still in beta."

### Variant A — `serializationLibrary=built_value` (the generator's default)

```text
GET /api/Courses
        ↓
CoursesApi.apiCoursesGet() -> Future<Response<BuiltList<CourseResponse>>>
        ↓
CourseResponse (built_value model, id/title/lessonCount)
```

- Valid, well-formed Dart. `CoursesApi` takes a `Dio` + `Serializers` instance via its
  constructor — plain constructor injection, no singletons, no code-gen magic needed to use it.
- `ManLearningApiClient` (the generated composition root) owns a `Dio` instance, exposes
  `basePathOverride`, a custom `interceptors` list, and already scaffolds
  `OAuthInterceptor` / `BasicAuthInterceptor` / `BearerAuthInterceptor` / `ApiKeyAuthInterceptor`
  with `setBearerAuth(name, token)` / `removeBearerAuth(name)` methods.
- Full sample saved at
  `docs/spikes/openapi-dart-client-generation/sample-generated/built_value/`.

### Variant B — `serializationLibrary=json_serializable` (marked `[BETA]` by the generator itself)

- Generation did not fail, but the **output is broken**: `CourseResponseLessonCount` (see
  "Problems" below) generated with an empty, syntactically incomplete equality/hashCode block
  (literally unterminated statements before `@override int get hashCode =>`). This is not usable
  as-is. This confirms the generator's own `[BETA]` label on this option.
- Not saved as a long-term sample; this result is reported here as the reason it was rejected,
  per the Spike's "generation result not good enough → try one more candidate" instruction.

## Problems

### 1. `int32` fields are generated as `anyOf: [integer, string]`, not a plain integer (real, reproducible issue)

The captured OpenAPI document represents `CourseResponse.lessonCount` (a plain C# `int`) as:

```json
"lessonCount": {
  "pattern": "^-?(?:0|[1-9]\\d*)$",
  "type": ["integer", "string"],
  "format": "int32"
}
```

(equivalently `anyOf: [{type: integer}, {type: string}]` when the document is forced to OpenAPI
`3.0` — this was verified directly: temporarily setting
`AddOpenApi(options => options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0)` in
`Program.cs`, capturing the document again, and reverting the change afterwards — `git diff` on
`Program.cs` shows no residual change). This is **ASP.NET Core's own native OpenAPI document
generator** (`Microsoft.AspNetCore.OpenApi` / `Microsoft.OpenApi` 2.x) choosing to describe
`int32` this way in both 3.0 and 3.1 output, not an artifact of the 3.1-beta warning.

Effect on generation: OpenAPI Generator cannot map `anyOf[integer,string]` to a plain Dart `int`.
It instead synthesizes a wrapper type, `CourseResponseLessonCount`, holding an `AnyOf` of
`int`/`String`, and every consumer of `CourseResponse.lessonCount` has to unwrap it instead of
reading a plain `int`. Confirmed root cause directly: regenerating from a **local-only, scratch
copy** of the captured document with `lessonCount` manually simplified to a plain
`{"type": "integer", "format": "int32"}` (never written back to the real API or committed) produced
a clean `int get lessonCount` field with no wrapper type — isolating the problem to the
"stringified integer" schema shape, not to the generator or to `CourseResponse` itself.

This is a real integration friction point between ASP.NET Core 10's native OpenAPI output and
OpenAPI Generator's Dart target, independent of which serialization library is chosen. It will
affect every `int`/`long` field in future endpoints, not just `lessonCount`.

#### Resolution (follow-up work, see root `int32 → anyOf` fix task)

**Root cause**: System.Text.Json's default `JsonNumberHandling` allows integer properties to be
deserialized from either a JSON number *or* a quoted JSON string (e.g. both `2` and `"2"` parse
into an `int`). ASP.NET Core 10's native OpenAPI document generator derives its schemas from
`JsonSchemaExporter`, which accurately reflects that permissiveness in the document — producing
`anyOf: [integer, string]` for *every* int/int32 property, not specifically `lessonCount`. This
is confirmed by Microsoft's own documentation/community write-ups of the ASP.NET Core 10 OpenAPI
integration and is not specific to `CourseResponse` or to this repository.

**Fix applied** (`src/ManLearning.Api/Program.cs`):

```csharp
builder.Services.ConfigureHttpJsonOptions(
    options => options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict);
```

This is the officially-documented, root-cause-level fix (not a schema patch): it changes what the
API actually accepts (no more quoted-string numbers), so the OpenAPI document's `integer` schema
and the real runtime contract agree. It is applied once, globally, in `Program.cs` — it is not
scoped to `lessonCount` or to any single field name, so it protects every current and future
int/int32 property in the API (e.g. a future `xp`, `level`, `score`, `questionCount` field) without
further changes.

An `IOpenApiSchemaTransformer` (ASP.NET Core's official schema-transformer extension point) was
considered as an alternative, but rejected: a transformer would only rewrite the *document* to look
like `integer`, while leaving the runtime free to still accept quoted-string numbers — i.e. the
document would then describe a stricter contract than the API actually enforces. Fixing the
`JsonSerializerOptions` instead makes the declared contract and the real behavior consistent,
which is the more correct fix.

**Before**:
```text
C# int
   ↓
OpenAPI: { "type": ["integer", "string"], "format": "int32", "pattern": "^-?(?:0|[1-9]\\d*)$" }
   ↓
Dart: synthesized `CourseResponseLessonCount` wrapper holding `AnyOf<int, String>`
```

**After**:
```text
C# int
   ↓
OpenAPI: { "type": "integer", "format": "int32" }
   ↓
Dart: plain `int get lessonCount;` — no wrapper type generated at all
```

Verified directly: rebuilt `ManLearning.Api`, re-captured `/openapi/v1.json`, confirmed the
`anyOf`/`pattern` are gone and `lessonCount` is a plain `{"type": "integer", "format": "int32"}`,
then re-ran `openapi-generator-cli generate -g dart-dio` (same `7.25.0`, same
`serializationLibrary=built_value` as originally selected) against the regenerated document — the
output no longer contains a `CourseResponseLessonCount` wrapper file at all, and `CourseResponse`
declares `int get lessonCount;` directly. `GET /api/courses` continues to return `200 OK` with the
same JSON payload shape. A regression test,
`tests/ManLearning.Api.Tests/OpenApiSchemaTests.cs`, now asserts generically (for every
int32-formatted property found in the OpenAPI document, not only `lessonCount`) that the schema
has no `anyOf`/`pattern` and declares a plain `"integer"` type — so this does not silently regress
if `JsonNumberHandling` is ever changed back, or if a new integer field is added with different
serialization settings somewhere. The refreshed sample in
`docs/spikes/openapi-dart-client-generation/sample-generated/built_value/` and the OpenAPI capture
in `docs/spikes/openapi-dart-client-generation/openapi/openapi.json` both reflect this fixed
state.

### 2. `json_serializable` mode for `dart-dio` is explicitly beta and broke on this schema

See Variant B above — not usable without the generator team fixing it, or without avoiding
`anyOf` schemas in the contract.

### 3. OpenAPI 3.1 is "beta" in OpenAPI Generator 7.25.0

The CLI prints `OpenAPI 3.1 support is still in beta` for every run against the native 3.1.1
document ASP.NET Core 10 produces. Generation still succeeded and produced valid Dart (Variant A),
so this was not a blocking problem in this one-endpoint spike, but it is a reason to keep
re-validating generation whenever the contract grows (more endpoints, nested objects, enums).

## Integration assessment

| Area | Result | Notes |
| --- | --- | --- |
| **Dio** | PASS | `CoursesApi` takes a `Dio` instance directly in its constructor; `ManLearningApiClient` owns configuration (`basePathOverride`, timeouts, `interceptors` list) that an app can fully override. No UI/global-state coupling in the generated code. |
| **Repository boundary** | PASS | Nothing in the generated code requires it to be called from UI. A `CoursesRepository` wrapping `CoursesApi`/`ManLearningApiClient` and exposing a domain-shaped return type is a direct, unobstructed wrap. |
| **Riverpod** | PASS (by inspection, not implemented) | Because `CoursesApi`/`ManLearningApiClient` are plain constructor-injected classes with no static/global state, they can be instantiated inside a Riverpod provider (e.g. `coursesApiProvider` → `coursesRepositoryProvider`) without adapter code. Not wired up in this spike. |
| **Auth header (`Authorization: Bearer`)** | PASS (plumbing exists, unexercised) | `BearerAuthInterceptor` + `ManLearningApiClient.setBearerAuth(name, token)` already exist in generated output. Because `GET /api/courses` currently has no `securitySchemes` in the OpenAPI document (no auth yet, by design), the interceptor has nothing to attach a token to yet — this will need re-verification once ADR-0007 authentication exists and the OpenAPI document gains a `bearerAuth` security scheme. |
| **Nullability** | NOT EXERCISED | `CourseResponse` has no nullable fields today. No nullable field was added to force a test, per instruction. The generated model types are null-safe Dart regardless (`String`, not `String?`, for the current required fields), so basic null-safety support is visible, just not nullable-field mapping specifically. |
| **Enum** | NOT EXERCISED | No enum exists in the current contract. Deferred to whenever the contract first introduces one. |
| **Error handling** | NOT EXERCISED (structurally) | No documented error schema exists yet (ADR-0008 defers this). What *is* visible: Dio surfaces non-2xx responses and deserialization failures as `DioException`, which a repository layer can catch and translate into an app-level error state. The generated code's exception path was inspected (`courses_api.dart` wraps deserialization failures in `DioException`), but no error-response schema exists to generate a typed error model from. |
| **int/long fields** | **PASS** (previously FAIL, see Problems #1 → Resolution) | Originally generated as an `AnyOf<int, String>` wrapper. Fixed by configuring `JsonNumberHandling.Strict` in `Program.cs`; `CourseResponse.lessonCount` now generates as a plain Dart `int`, verified by regeneration and protected by `tests/ManLearning.Api.Tests/OpenApiSchemaTests.cs`. |

## Generated code management: recommendation

Three options were weighed, no final policy was declared (not required at spike stage):

- **Option A — commit generated code** (e.g. `lib/generated/`): guarantees reproducible builds
  without a generation step in CI, but generated code drifts from the contract silently if someone
  forgets to regenerate after an API change, and diffs on regeneration are large and noisy.
- **Option B — commit only the OpenAPI spec + generator command, generate in CI/locally**: keeps
  the repository free of auto-generated noise and forces regeneration to be an explicit, visible
  step tied to contract changes; requires Java (or an equivalent) in the Flutter build/CI pipeline,
  which is a new dependency to manage.
- **Option C — vendor the generator config as a pinned version + spec snapshot, regenerate as a
  documented manual step before each Flutter release build**: a middle ground — avoids committing
  generated code, but doesn't require wiring generation into CI immediately while the project is
  still pre-Flutter.

**Recommendation**: Option B (spec + pinned generator version/command committed; generated code
not committed), once a real Flutter project exists. It matches ADR-0008's "OpenAPI document as the
single source of truth" principle most directly — the generated Dart code is a build artifact of
that source of truth, not a second thing to keep in sync by hand. This is a recommendation only;
ADR-0008 is not being amended by this spike.

## Decision

**Candidate for the next phase: OpenAPI Generator's `dart-dio` generator, `serializationLibrary:
built_value` (the default), OpenAPI Generator CLI pinned at `7.25.0`.**

Reasoning:
- It is the only candidate actually exercised here that produced valid, Dio-based, null-safe Dart
  from the real `GET /api/courses` contract.
- It does not require a pre-existing Flutter/Dart project, so it was usable within this spike's
  constraints.
- `openapi_generator` (the pub/build_runner wrapper) remains the recommended **follow-up**
  evaluation once an actual Flutter project exists, since it would fold generation into the normal
  Dart build pipeline rather than a separate CLI invocation — but it uses the same underlying
  OpenAPI Generator engine, so the `int32`-as-`anyOf` problem (Problems #1) will reproduce there
  too and should be resolved or explicitly worked around before relying on it.

Before this generator choice is adopted for real Flutter work, Problems #1 should be resolved —
either by finding an ASP.NET Core / `Microsoft.OpenApi` configuration that emits a plain
`integer` schema for `int32` fields (preferred, keeps the contract idiomatic), or by accepting the
`AnyOf` wrapper type and writing a small shared unwrap helper in the Flutter repository layer.
**This has since been resolved** — see "Resolution" under Problems #1 — by configuring
`JsonNumberHandling.Strict` in `ManLearning.Api`'s `Program.cs`, which was the preferred option of
the two listed here.

## Reproducing this spike

```bash
# 1. Run the API and capture its live OpenAPI document (Program.cs now configures strict JSON
#    number handling, so the captured document already has plain `integer` schemas — no extra
#    step is needed here).
dotnet run --project src/ManLearning.Api --no-build --urls http://localhost:5299 &
curl -s http://localhost:5299/openapi/v1.json -o openapi.json

# 2. Generate a Dart Dio client (requires a JVM; no Dart/Flutter SDK needed just to generate)
npx @openapitools/openapi-generator-cli generate \
  -i openapi.json \
  -g dart-dio \
  -o out/ \
  --additional-properties='pubName=man_learning_api_client'
```

## Artifacts

- `docs/spikes/openapi-dart-client-generation.md` — this document.
- `docs/spikes/openapi-dart-client-generation/openapi/openapi.json` — the real OpenAPI document
  captured from the running `ManLearning.Api` (not hand-written), reflecting the fixed
  `integer`/`int32` schema for `lessonCount`.
- `docs/spikes/openapi-dart-client-generation/sample-generated/built_value/` — a trimmed sample of
  the selected candidate's generated output (`course_response.dart`, `courses_api.dart`,
  `api.dart`, `pubspec.yaml`), regenerated after the fix — `lessonCount` is now a plain `int` and
  no `course_response_lesson_count.dart` wrapper file exists anymore. Kept as evidence only — not
  referenced by, or built as part of, any `src/`/`tests/` project.
- `src/ManLearning.Api/Program.cs` — `ConfigureHttpJsonOptions` with
  `JsonNumberHandling.Strict`, the actual fix for the `int32 → anyOf` problem.
- `tests/ManLearning.Api.Tests/OpenApiSchemaTests.cs` — regression test asserting every
  int32-formatted property in the OpenAPI document is a plain `integer` schema (no `anyOf`
  union), generically across the whole document rather than hardcoded to `lessonCount`.

## Unresolved (intentionally left open)

- ~~Problems #1 (`int32` → `anyOf[integer,string]`)~~ — **resolved**, see "Resolution" under
  Problems #1 (`JsonNumberHandling.Strict` configured in `Program.cs`, verified by regeneration,
  protected by `tests/ManLearning.Api.Tests/OpenApiSchemaTests.cs`).
- Nullability and enum mapping remain unverified against a real schema — revisit the first time
  either appears in the API contract.
- Error-response schema/contract (and its corresponding generated error model) is not designed;
  ADR-0008 already defers this.
- Auth-header behavior (`BearerAuthInterceptor` actually firing) is unverified against a live
  `securitySchemes` entry — revisit once ADR-0007 authentication lands and the OpenAPI document
  gains a security scheme.
- Final generated-code management policy (Option A/B/C) is a recommendation only, not adopted as
  an ADR amendment.

