using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ExpenseFlow.Identity.Domain.Interfaces;
using ExpenseFlow.Identity.Application.Interfaces.Messaging;
using ExpenseFlow.Identity.Persistence.Repositories;

namespace ExpenseFlow.Identity.Persistence.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        return services;
    }
}
