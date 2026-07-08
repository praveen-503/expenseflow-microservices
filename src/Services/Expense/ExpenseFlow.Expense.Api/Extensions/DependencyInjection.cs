using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using ExpenseFlow.Expense.Application.Extensions;
using ExpenseFlow.Expense.Infrastructure.Extensions;
using ExpenseFlow.Expense.Persistence.Extensions;

namespace ExpenseFlow.Expense.Api.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationServices();
        services.AddInfrastructureServices(configuration);
        services.AddPersistenceServices(configuration);

        return services;
    }
}
