using ManLearning.Application.Learning;
using ManLearning.Infrastructure;
using ManLearning.Infrastructure.Persistence.EfCore;
using ManLearning.Web.Components;
using ManLearning.Web.Learners;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddManLearningInfrastructure(builder.Configuration);
builder.Services.AddScoped<CourseCatalogService>();
builder.Services.AddScoped<LessonProgressService>();
builder.Services.AddScoped<QuizAttemptService>();
builder.Services.AddScoped<LearnerDashboardService>();
builder.Services.AddScoped<CurrentLearnerContext>();

var app = builder.Build();

// Prepare the schema (migrate on SQL Server, ensure-created on the local SQLite fallback) and
// seed the demo course catalog on startup. This is an explicit, synchronous step rather than a
// background job because the current scale does not warrant a separate release/migration
// pipeline yet (see ADR 0004's "follow-up work").
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ManLearningDbContext>();
    await ManLearningDbInitializer.InitializeAsync(dbContext);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
