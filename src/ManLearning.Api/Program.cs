using System.Text.Json.Serialization;
using ManLearning.Application.Learning;
using ManLearning.Infrastructure;
using ManLearning.Infrastructure.Persistence.EfCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Flutter Web (served from its own local dev origin, e.g. `flutter run -d chrome`) calls this API
// from a different origin than the API itself, so the browser enforces CORS. Only the Development
// environment gets a CORS policy, and only for explicitly configured local dev origins (see
// "FlutterWeb:DevOrigins" in appsettings.Development.json) — this is not a production CORS
// policy (no wildcard origin, no production Azure Static Web Apps origin yet; that is a separate,
// later decision per ADR-0010).
const string FlutterWebDevCorsPolicy = "FlutterWebDev";
var flutterWebDevOrigins = builder.Configuration.GetSection("FlutterWeb:DevOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
{
    options.AddPolicy(FlutterWebDevCorsPolicy, policy =>
    {
        if (flutterWebDevOrigins.Length > 0)
        {
            policy.WithOrigins(flutterWebDevOrigins).AllowAnyHeader().AllowAnyMethod();
        }
    });
});

// By default, System.Text.Json (and therefore ASP.NET Core's native OpenAPI document generator,
// which derives schemas from JsonSchemaExporter) allows integer properties to also be read from
// quoted JSON strings (e.g. both `2` and `"2"` deserialize into an `int`). That permissiveness is
// what the generator is actually describing when it emits `anyOf: [integer, string]` for every
// int/int32 property instead of a plain `integer` schema (see
// docs/spikes/openapi-dart-client-generation.md). Strongly-typed OpenAPI client generators (e.g.
// OpenAPI Generator's dart-dio target) cannot map that union to a plain Dart `int` and instead
// synthesize an `AnyOf<int, String>` wrapper type for every integer field in the contract.
// Declaring strict number handling removes the ambiguity at its source — the API genuinely never
// needs or wants to accept a quoted number — so the generated schema and the real runtime
// contract agree, for every current and future int-typed field, not just one property.
builder.Services.ConfigureHttpJsonOptions(
    options => options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Same composition pattern ManLearning.Web already uses (ADR-0006): the API depends on
// Application services directly and reuses Infrastructure's registration helper so both hosts
// share one source of truth for how repositories/DbContext are wired, rather than duplicating it.
builder.Services.AddManLearningInfrastructure(builder.Configuration);
builder.Services.AddScoped<CourseCatalogService>();

var app = builder.Build();

// Prepare the schema and seed the demo course catalog on startup, mirroring ManLearning.Web's
// Program.cs so the API can be run standalone against the same local SQLite fallback or Azure SQL
// Database (ADR-0004) without a separate migration step.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ManLearningDbContext>();
    await ManLearningDbInitializer.InitializeAsync(dbContext);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseCors(FlutterWebDevCorsPolicy);
}

// No authentication/authorization middleware yet: ADR-0007 (Microsoft Entra External ID) is a
// separate, not-yet-implemented decision. GET /api/courses is intentionally anonymous-accessible
// for this first vertical slice.
app.UseAuthorization();

app.MapControllers();

app.Run();

/// <summary>
/// Partial Program class so <c>WebApplicationFactory&lt;Program&gt;</c> can boot this API host
/// in-process for integration tests, consistent with ASP.NET Core's top-level statement testing
/// convention.
/// </summary>
public partial class Program;
