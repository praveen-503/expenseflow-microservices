using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseFlow.Identity.Api.Extensions;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddHealthCheckConfig(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("IdentityDb") 
            ?? "Server=localhost;Database=ExpenseFlowIdentity;Trusted_Connection=True;TrustServerCertificate=True;";

        services.AddHealthChecks()
            .AddSqlServer(connectionString, name: "SQLServer-Check", tags: new[] { "db", "sqlserver" });

        return services;
    }

    public static IApplicationBuilder MapHealthChecksConfig(this WebApplication app)
    {
        app.MapHealthChecks("/health");
        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("db") || check.Tags.Contains("servicebus")
        });

        return app;
    }
}
