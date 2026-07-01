using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using ExpenseFlow.Notification.Application.Extensions;
using ExpenseFlow.Notification.Infrastructure.Extensions;

namespace ExpenseFlow.Notification.Api.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationServices();
        services.AddInfrastructureServices(configuration);

        // Dynamically load and register Persistence Services to respect Clean Architecture dependency rules
        try
        {
            var assemblyName = "ExpenseFlow.Notification.Persistence";
            var assembly = Assembly.Load(assemblyName);
            
            var type = assembly.GetType("ExpenseFlow.Notification.Persistence.Extensions.DependencyInjection") 
                ?? throw new InvalidOperationException($"Persistence DI class not found in {assemblyName}.");
                
            var method = type.GetMethod("AddPersistenceServices", BindingFlags.Public | BindingFlags.Static) 
                ?? throw new InvalidOperationException("AddPersistenceServices method not found.");
                
            method.Invoke(null, new object[] { services, configuration });
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to dynamically load and register Persistence Services.", ex);
        }

        return services;
    }
}
