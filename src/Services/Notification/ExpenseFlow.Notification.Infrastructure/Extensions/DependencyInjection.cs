using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ExpenseFlow.Notification.Application.Interfaces.Messaging;
using ExpenseFlow.Notification.Infrastructure.Messaging;

namespace ExpenseFlow.Notification.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEventPublisher, AzureServiceBusPublisher>();
        return services;
    }
}
