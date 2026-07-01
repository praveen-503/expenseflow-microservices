using Microsoft.Extensions.DependencyInjection;
using ExpenseFlow.Identity.Api.Middleware;

namespace ExpenseFlow.Identity.Api.Extensions;

public static class ProblemDetailsExtensions
{
    public static IServiceCollection AddProblemDetailsConfig(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
    }
}
