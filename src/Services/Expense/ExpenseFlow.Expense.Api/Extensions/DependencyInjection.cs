using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ExpenseFlow.Expense.Application.Extensions;
using ExpenseFlow.Expense.Persistence.Extensions;
using ExpenseFlow.Expense.Infrastructure.Extensions;

namespace ExpenseFlow.Expense.Api.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationServices();
        services.AddPersistenceServices(configuration);
        services.AddInfrastructureServices(configuration);

        return services;
    }
}
