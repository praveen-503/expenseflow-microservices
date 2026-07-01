using Microsoft.Extensions.DependencyInjection;
using ExpenseFlow.Expense.Api.Middleware;

namespace ExpenseFlow.Expense.Api.Extensions;

public static class ProblemDetailsExtensions
{
    public static IServiceCollection AddProblemDetailsConfig(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
    }
}
