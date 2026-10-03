using ManLearning.Application.Learning;
using ManLearning.Infrastructure;
using ManLearning.Web.Components;
using ManLearning.Web.Learners;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddManLearningInfrastructure();
builder.Services.AddScoped<CourseCatalogService>();
builder.Services.AddScoped<LessonProgressService>();
builder.Services.AddScoped<QuizAttemptService>();
builder.Services.AddScoped<LearnerDashboardService>();
builder.Services.AddScoped<CurrentLearnerContext>();

var app = builder.Build();

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
