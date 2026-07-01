using Microsoft.Extensions.DependencyInjection;
using ExpenseFlow.Notification.Api.Middleware;

namespace ExpenseFlow.Notification.Api.Extensions;

public static class ProblemDetailsExtensions
{
    public static IServiceCollection AddProblemDetailsConfig(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
    }
}
