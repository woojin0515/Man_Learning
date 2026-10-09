#!/usr/bin/env bash
# Regenerates the Dart API client package at flutter/man_learning_api_client/ from the real
# ManLearning.Api OpenAPI document.
#
# This script is the single source of truth for how the generated client is produced (ADR-0008 /
# docs/spikes/openapi-dart-client-generation.md). The generated output is a build artifact, not
# hand-written code: do not edit files under flutter/man_learning_api_client/ directly, re-run
# this script instead.
#
# Why a separate package instead of a `lib/generated/` folder inside the app: OpenAPI Generator's
# dart-dio target produces a complete, self-contained Dart package (its own pubspec.yaml,
# analysis_options.yaml, and a build.yaml driving built_value's build_runner codegen). Nesting a
# second pubspec.yaml inside the Flutter app's own `lib/` directory is not valid Dart package
# structure. Instead, flutter/man_learning_api_client is a sibling package, and
# flutter/man_learning depends on it via a local path dependency
# (`man_learning_api_client: {path: ../man_learning_api_client}`), the same pattern Dart/Flutter
# monorepos commonly use for generated/internal packages.
#
# Prerequisites:
#   - A JVM on PATH (OpenAPI Generator CLI runs as a Java process). Tested with Homebrew
#     `openjdk` (java 26).
#   - Node.js/npm on PATH (used only to run the pinned `@openapitools/openapi-generator-cli` via
#     `npx`, no global install required).
#   - Flutter/Dart SDK on PATH (used to run `dart run build_runner build` for the generated
#     package's built_value serializers).
#   - ManLearning.Api running locally so its live OpenAPI document can be captured. The script
#     does not start the API itself.
#
# Usage:
#   ./tool/generate-dart-client.sh [api-base-url]
#
#   api-base-url defaults to http://localhost:5299 (the port used throughout
#   docs/spikes/openapi-dart-client-generation.md and the API vertical slice tests).
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"

API_BASE_URL="${1:-http://localhost:5299}"
GENERATOR_VERSION="7.25.0"
OUTPUT_DIR="$REPO_ROOT/flutter/man_learning_api_client"
SPEC_FILE="$REPO_ROOT/docs/spikes/openapi-dart-client-generation/openapi/openapi.json"

echo "Capturing OpenAPI document from $API_BASE_URL/openapi/v1.json ..."
curl --fail --silent --show-error "$API_BASE_URL/openapi/v1.json" -o "$SPEC_FILE"

echo "Generating Dart client (OpenAPI Generator CLI $GENERATOR_VERSION, dart-dio, built_value) ..."
rm -rf "$OUTPUT_DIR"
# The npm wrapper package (@openapitools/openapi-generator-cli) is versioned independently from
# the underlying Java generator JAR it downloads and runs. Pinning the JAR version is done via
# the OPENAPI_GENERATOR_VERSION environment variable (not an npm package version suffix — no
# npm release named "7.25.0" exists), so the actual generator logic is reproducible even as the
# npm wrapper itself updates.
OPENAPI_GENERATOR_VERSION="$GENERATOR_VERSION" npx --yes @openapitools/openapi-generator-cli generate \
  -i "$SPEC_FILE" \
  -g dart-dio \
  -o "$OUTPUT_DIR" \
  --additional-properties="pubName=man_learning_api_client,serializationLibrary=built_value"

echo "Running build_runner to produce built_value serializers (*.g.dart) ..."
(cd "$OUTPUT_DIR" && dart pub get && dart run build_runner build --delete-conflicting-outputs)

echo "Done. Generated client is at $OUTPUT_DIR"
