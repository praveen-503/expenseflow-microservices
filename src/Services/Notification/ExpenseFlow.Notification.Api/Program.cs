using ExpenseFlow.Notification.Api.Extensions;
using Azure.Identity;
using System;
using Microsoft.AspNetCore.RateLimiting;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Register Azure Key Vault Configuration Provider
    var vaultUriStr = builder.Configuration["AzureKeyVault:VaultUri"];
    if (!string.IsNullOrEmpty(vaultUriStr))
    {
        try
        {
            builder.Configuration.AddAzureKeyVault(new Uri(vaultUriStr), new DefaultAzureCredential());
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error registering Azure Key Vault '{vaultUriStr}': {ex.Message}");
        }
    }

    // Configure Serilog Logging
    builder.Host.UseSerilogLogging();

    // Register Telemetry Configuration
    builder.Services.AddTelemetryConfig(builder.Configuration, "ExpenseFlow.Notification.Api");

    // Configure Rate Limiting
    builder.Services.AddRateLimiter(options =>
    {
        options.RejectionStatusCode = Microsoft.AspNetCore.Http.StatusCodes.Status429TooManyRequests;
        options.AddFixedWindowLimiter("fixed", opt =>
        {
            opt.Window = TimeSpan.FromSeconds(10);
            opt.PermitLimit = 100;
            opt.QueueLimit = 10;
            opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
        });
    });

    // Configure Response Compression
    builder.Services.AddResponseCompression(options =>
    {
        options.EnableForHttps = true;
        options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.BrotliCompressionProvider>();
        options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProvider>();
    });

    // Add services to the container.
    builder.Services.AddApiServices(builder.Configuration);
    builder.Services.AddControllers();
    builder.Services.AddApiVersioningConfig();
    builder.Services.AddSwaggerConfig();
    builder.Services.AddAuthServices(builder.Configuration);
    builder.Services.AddHealthCheckConfig(builder.Configuration);
    builder.Services.AddProblemDetailsConfig();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseCorrelationId();
    app.UseResponseCompression();
    app.UseRateLimiter();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwaggerConfig();
        app.MapOpenApi();
        app.MapGet("/", context =>
        {
            context.Response.Redirect("/swagger");
            return System.Threading.Tasks.Task.CompletedTask;
        });
    }

    app.UseExceptionHandler(); // Maps native IExceptionHandler (GlobalExceptionHandler)
    app.UseStatusCodePages(); // ProblemDetails integration

    app.UseHttpsRedirection();

    app.UseCors("CorsPolicy");

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers().RequireRateLimiting("fixed");
    app.MapHealthChecksConfig();

    // Apply pending migrations automatically on startup
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();
        try
        {
            var appDbContext = services.GetService<ExpenseFlow.Notification.Application.Interfaces.IApplicationDbContext>();
            if (appDbContext is Microsoft.EntityFrameworkCore.DbContext dbContext)
            {
                logger.LogInformation("Checking and applying pending database migrations for Notification service...");
                await Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions.MigrateAsync(dbContext.Database);
                logger.LogInformation("Notification service database migrations checked/applied successfully.");
            }
            else
            {
                logger.LogWarning("IApplicationDbContext could not be resolved as DbContext. Skipping database migrations.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while applying Notification service database migrations. Continuing startup.");
        }
    }

    app.Run();
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Unhandled exception during startup: {ex}");
    try
    {
        System.IO.File.WriteAllText("startup_crash_log.txt", ex.ToString());
    }
    catch { }
    throw;
}

