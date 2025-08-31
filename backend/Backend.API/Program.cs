using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Backend.Infrastructure.Persistence;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Controllers + Newtonsoft JSON (or switch to System.Text.Json if preferred)
builder.Services.AddControllers()
    .AddNewtonsoftJson();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(o => o.AddPolicy("frontend", p => p
    .WithOrigins("http://localhost:5173", "http://localhost:3000")
    .AllowAnyHeader()
    .AllowAnyMethod()));

// API versioning
builder.Services.AddApiVersioning(o =>
{
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.ReportApiVersions = true;
})
.AddApiExplorer(o =>
{
    o.GroupNameFormat = "'v'VVV"; // e.g. v1
    o.SubstituteApiVersionInUrl = true;
});

// EF Core
var cs = builder.Configuration.GetConnectionString("Default") ?? "Data Source=app.db";
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite(cs));

// Health checks (requires Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore in Backend.API)
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>();

// Validation + ProblemDetails
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddProblemDetails(o =>
{
    o.IncludeExceptionDetails = (ctx, ex) => builder.Environment.IsDevelopment();
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var desc in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());
        }
    });
}

// Health check endpoint with JSON response
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var json = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            results = report.Entries.Select(e => new
            {
                component = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration = e.Value.Duration.ToString()
            })
        });
        await context.Response.WriteAsync(json);
    }
});

app.UseHttpsRedirection();
app.UseCors("frontend");

// ProblemDetails error formatting
app.UseProblemDetails();

app.MapControllers();
app.Run();
