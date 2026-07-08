using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Azure.Messaging.ServiceBus;
using Azure.Identity;
using System;
using ExpenseFlow.Identity.Application.Interfaces;
using ExpenseFlow.Identity.Application.Interfaces.Messaging;
using ExpenseFlow.Identity.Infrastructure.Security;
using ExpenseFlow.Identity.Infrastructure.Services;
using ExpenseFlow.Identity.Infrastructure.Messaging;
using ExpenseFlow.Identity.Infrastructure.HealthChecks;

namespace ExpenseFlow.Identity.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        // Service Bus Configuration
        services.Configure<ServiceBusOptions>(configuration.GetSection(ServiceBusOptions.SectionName));

        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<ServiceBusOptions>>().Value;
            var clientOptions = new ServiceBusClientOptions
            {
                RetryOptions = new ServiceBusRetryOptions
                {
                    Mode = options.Retry.Mode.Equals("Fixed", StringComparison.OrdinalIgnoreCase) 
                        ? ServiceBusRetryMode.Fixed 
                        : ServiceBusRetryMode.Exponential,
                    MaxRetries = options.Retry.MaxRetries,
                    Delay = TimeSpan.FromSeconds(options.Retry.DelaySeconds),
                    MaxDelay = TimeSpan.FromSeconds(options.Retry.MaxDelaySeconds)
                }
            };

            // Use Managed Identity (FullyQualifiedNamespace) if configured; fallback to ConnectionString
            if (!string.IsNullOrEmpty(options.FullyQualifiedNamespace))
            {
                return new ServiceBusClient(options.FullyQualifiedNamespace, new DefaultAzureCredential(), clientOptions);
            }

            return new ServiceBusClient(options.ConnectionString, clientOptions);
        });

        services.AddSingleton<IEventPublisher, AzureServiceBusPublisher>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        services.AddHealthChecks()
            .AddCheck<AzureServiceBusHealthCheck>("AzureServiceBus-Check", tags: new[] { "ready", "servicebus" });

        return services;
    }
}
