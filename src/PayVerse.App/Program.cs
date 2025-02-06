using DotNetEnv;
using PayVerse.App.Configurations;
using PayVerse.App.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Load the .env file
Env.Load();

builder.Services
    .InstallServices(
        builder.Configuration,
        typeof(IServiceInstaller).Assembly);

builder.Host.UseSerilog((context, configuration) =>
    configuration
        .ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

// Register the global exception handling middleware in the request processing pipeline.
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();