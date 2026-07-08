using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using ExpenseFlow.Notification.Application.Extensions;
using ExpenseFlow.Notification.Infrastructure.Extensions;
using ExpenseFlow.Notification.Persistence.Extensions;

namespace ExpenseFlow.Notification.Api.Extensions;

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
