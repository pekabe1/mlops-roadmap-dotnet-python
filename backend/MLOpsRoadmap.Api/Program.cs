using MLOpsRoadmap.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()));

builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<CourseService>();
builder.Services.AddSingleton<ProgressService>();

var app = builder.Build();

app.UseCors();

// Initialize SQLite database
var progressService = app.Services.GetRequiredService<ProgressService>();
progressService.Initialize();

// Health check
app.MapGet("/api/health", () => Results.Ok(new { status = "ok" }));

// Course endpoints
app.MapGet("/api/course", (CourseService cs) =>
    Results.Ok(cs.GetModules()));

app.MapGet("/api/course/{id}", (string id, CourseService cs) =>
{
    var module = cs.GetModuleDetail(id);
    return module is null ? Results.NotFound() : Results.Ok(module);
});

// Progress endpoints
app.MapGet("/api/progress", (ProgressService ps) =>
    Results.Ok(ps.GetAll()));

app.MapPut("/api/progress/{moduleId}", (string moduleId, ProgressUpdate update, ProgressService ps) =>
{
    ps.Upsert(moduleId, update.Completed);
    return Results.Ok(new { moduleId, completed = update.Completed });
});

app.Run();

record ProgressUpdate(bool Completed);
