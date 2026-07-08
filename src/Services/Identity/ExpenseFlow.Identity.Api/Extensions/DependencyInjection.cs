using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using ExpenseFlow.Identity.Application.Extensions;
using ExpenseFlow.Identity.Infrastructure.Extensions;
using ExpenseFlow.Identity.Persistence.Extensions;

namespace ExpenseFlow.Identity.Api.Extensions;

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
