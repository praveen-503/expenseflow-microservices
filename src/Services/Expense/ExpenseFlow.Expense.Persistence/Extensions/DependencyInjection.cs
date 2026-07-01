using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ExpenseFlow.Expense.Domain.Interfaces;
using ExpenseFlow.Expense.Persistence.Repositories;

namespace ExpenseFlow.Expense.Persistence.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        return services;
    }
}
