using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ExpenseFlow.Identity.Application.Interfaces;
using ExpenseFlow.Identity.Application.Interfaces.Messaging;
using ExpenseFlow.Identity.Infrastructure.Security;
using ExpenseFlow.Identity.Infrastructure.Services;
using ExpenseFlow.Identity.Infrastructure.Messaging;

namespace ExpenseFlow.Identity.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEventPublisher, AzureServiceBusPublisher>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        return services;
    }
}
