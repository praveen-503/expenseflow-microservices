using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ExpenseFlow.Notification.Application.Interfaces;
using ExpenseFlow.Notification.Application.Interfaces.Messaging;
using ExpenseFlow.Notification.Infrastructure.Messaging;
using ExpenseFlow.Notification.Infrastructure.Services;

namespace ExpenseFlow.Notification.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddSingleton<IEventPublisher, AzureServiceBusPublisher>();
        
        // Register Hosted background listening service
        services.AddHostedService<AzureServiceBusConsumerService>();

        return services;
    }
}
