using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ExpenseFlow.Notification.Application.Extensions;
using ExpenseFlow.Notification.Persistence.Extensions;
using ExpenseFlow.Notification.Infrastructure.Extensions;

namespace ExpenseFlow.Notification.Api.Extensions;

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
