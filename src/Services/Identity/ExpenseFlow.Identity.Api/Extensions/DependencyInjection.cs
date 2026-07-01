using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ExpenseFlow.Identity.Application.Extensions;
using ExpenseFlow.Identity.Persistence.Extensions;
using ExpenseFlow.Identity.Infrastructure.Extensions;

namespace ExpenseFlow.Identity.Api.Extensions;

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
