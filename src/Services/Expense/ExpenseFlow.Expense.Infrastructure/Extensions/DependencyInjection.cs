using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Azure.Messaging.ServiceBus;
using System;
using ExpenseFlow.Expense.Application.Interfaces;
using ExpenseFlow.Expense.Application.Interfaces.Messaging;
using ExpenseFlow.Expense.Infrastructure.Messaging;
using ExpenseFlow.Expense.Infrastructure.Services;
using ExpenseFlow.Expense.Infrastructure.HealthChecks;

namespace ExpenseFlow.Expense.Infrastructure.Extensions;

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
            return new ServiceBusClient(options.ConnectionString, clientOptions);
        });

        services.AddSingleton<IEventPublisher, AzureServiceBusPublisher>();

        services.AddHealthChecks()
            .AddCheck<AzureServiceBusHealthCheck>("AzureServiceBus-Check", tags: new[] { "ready", "servicebus" });

        return services;
    }
}
