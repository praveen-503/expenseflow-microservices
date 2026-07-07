using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using System;

namespace ExpenseFlow.Expense.Api.Extensions;

public static class TelemetryExtensions
{
    public static IServiceCollection AddTelemetryConfig(this IServiceCollection services, IConfiguration configuration, string serviceName)
    {
        var appInsightsConnectionString = configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"] 
            ?? configuration["ApplicationInsights:ConnectionString"];
        
        var openTelemetry = services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName));

        if (!string.IsNullOrEmpty(appInsightsConnectionString))
        {
            openTelemetry.UseAzureMonitor(options =>
            {
                options.ConnectionString = appInsightsConnectionString;
            });
        }

        var otlpEndpoint = configuration["Telemetry:OtlpEndpoint"];
        if (!string.IsNullOrEmpty(otlpEndpoint))
        {
            openTelemetry.WithTracing(tracing =>
            {
                tracing.AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri(otlpEndpoint);
                });
            });
        }

        return services;
    }
}
