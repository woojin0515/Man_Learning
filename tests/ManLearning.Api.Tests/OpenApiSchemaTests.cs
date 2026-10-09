using System.Net;
using System.Text.Json;

namespace ManLearning.Api.Tests;

/// <summary>
/// Regression test for the ASP.NET Core 10 native OpenAPI "stringly-typed integer" quirk
/// documented in <c>docs/spikes/openapi-dart-client-generation.md</c>: by default, ASP.NET
/// Core's OpenAPI document generator describes every <c>int</c>/<c>int32</c> property as
/// <c>anyOf: [integer, string]</c> because <see cref="System.Text.Json.Serialization.JsonNumberHandling"/>
/// defaults to allowing numbers to be read from quoted JSON strings. That union type cannot be
/// mapped to a plain <c>int</c> by Dart (or other strongly-typed) OpenAPI client generators —
/// OpenAPI Generator's <c>dart-dio</c> target instead synthesizes an <c>AnyOf&lt;int, String&gt;</c>
/// wrapper type. <c>Program.cs</c> configures strict JSON number handling to eliminate the union at
/// its source (the API never accepted stringified numbers in the first place, so this also makes
/// the contract and the runtime behavior consistent). This test asserts the fix generically for any
/// integer property, not only <c>CourseResponse.LessonCount</c>, so future integer fields (xp,
/// level, score, etc.) stay protected by the same assertion without edits.
/// </summary>
public sealed class OpenApiSchemaTests(ManLearningApiFactory factory) : IClassFixture<ManLearningApiFactory>
{
    [Fact]
    public async Task OpenApiDocument_IntegerSchemas_AreNotUnionedWithString()
    {
        var client = factory.CreateClient();

        var response = await client.GetAsync("/openapi/v1.json");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var schemas = document.RootElement.GetProperty("components").GetProperty("schemas");

        var checkedAnyIntegerProperty = false;
        foreach (var schema in schemas.EnumerateObject())
        {
            if (!schema.Value.TryGetProperty("properties", out var properties))
            {
                continue;
            }

            foreach (var property in properties.EnumerateObject())
            {
                if (!PropertyDeclaresInt32Format(property.Value))
                {
                    continue;
                }

                checkedAnyIntegerProperty = true;
                AssertIsPlainIntegerSchema(schema.Name, property.Name, property.Value);
            }
        }

        Assert.True(
            checkedAnyIntegerProperty,
            "Expected at least one int32 property (e.g. CourseResponse.lessonCount) in the OpenAPI " +
            "document to exercise this regression check; none were found.");
    }

    private static bool PropertyDeclaresInt32Format(JsonElement propertySchema) =>
        propertySchema.TryGetProperty("format", out var format) && format.GetString() == "int32";

    private static void AssertIsPlainIntegerSchema(string schemaName, string propertyName, JsonElement propertySchema)
    {
        Assert.False(
            propertySchema.TryGetProperty("anyOf", out _),
            $"{schemaName}.{propertyName} regressed back to an `anyOf` union schema instead of a plain integer.");
        Assert.False(
            propertySchema.TryGetProperty("pattern", out _),
            $"{schemaName}.{propertyName} still carries the string-coercion `pattern` constraint.");

        Assert.True(
            propertySchema.TryGetProperty("type", out var type),
            $"{schemaName}.{propertyName} is missing a `type` keyword.");

        // OpenAPI 3.1/JSON Schema allows `type` to be either a single string or an array of
        // strings. A nullable int property (e.g. ASP.NET Core's built-in `ProblemDetails.Status`,
        // or a future nullable domain field) legitimately produces `type: ["integer", "null"]` —
        // that is a different, valid construct from the `anyOf: [integer, string]` union this
        // test guards against, so both the single-type and the integer-or-null array forms are
        // accepted. Only a type array that still mixes in "string" would indicate the original
        // bug has regressed.
        if (type.ValueKind == JsonValueKind.Array)
        {
            var types = type.EnumerateArray().Select(t => t.GetString()).ToArray();
            Assert.True(
                types is [ "integer" ] or [ "integer", "null" ] or [ "null", "integer" ],
                $"{schemaName}.{propertyName} has type array {JsonSerializer.Serialize(types)}, expected only " +
                "\"integer\", optionally unioned with \"null\" for a nullable field.");
        }
        else
        {
            Assert.Equal("integer", type.GetString());
        }
    }
}
