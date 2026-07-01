using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseFlow.Notification.Api.Extensions;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddHealthCheckConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? "Server=localhost;Database=ExpenseFlowNotification;Trusted_Connection=True;TrustServerCertificate=True;";

        services.AddHealthChecks()
            .AddSqlServer(connectionString, name: "SQLServer-Check", tags: new[] { "db", "sqlserver" });

        return services;
    }

    public static IApplicationBuilder MapHealthChecksConfig(this WebApplication app)
    {
        app.MapHealthChecks("/health");
        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("db")
        });

        return app;
    }
}
