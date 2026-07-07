using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using System;

namespace ExpenseFlow.Notification.Functions.Extensions;

public static class TelemetryExtensions
{
    public static IServiceCollection AddTelemetryConfig(this IServiceCollection services, IConfiguration configuration, string serviceName)
    {
        var otlpEndpoint = configuration["Telemetry:OtlpEndpoint"];
        if (!string.IsNullOrEmpty(otlpEndpoint))
        {
            services.AddOpenTelemetry()
                .ConfigureResource(resource => resource.AddService(serviceName))
                .WithTracing(tracing =>
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
