using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ExpenseFlow.Expense.Application.Interfaces;
using ExpenseFlow.Expense.Application.Interfaces.Messaging;
using ExpenseFlow.Expense.Infrastructure.Messaging;
using ExpenseFlow.Expense.Infrastructure.Services;

namespace ExpenseFlow.Expense.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddSingleton<IEventPublisher, AzureServiceBusPublisher>();
        return services;
    }
}
