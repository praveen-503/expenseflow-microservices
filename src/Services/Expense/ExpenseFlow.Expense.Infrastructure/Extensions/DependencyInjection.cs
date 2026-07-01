using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ExpenseFlow.Expense.Application.Interfaces.Messaging;
using ExpenseFlow.Expense.Infrastructure.Messaging;

namespace ExpenseFlow.Expense.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEventPublisher, AzureServiceBusPublisher>();
        return services;
    }
}
