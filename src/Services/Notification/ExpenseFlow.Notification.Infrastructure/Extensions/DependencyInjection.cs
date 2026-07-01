using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace ExpenseFlow.Notification.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services;
    }
}
