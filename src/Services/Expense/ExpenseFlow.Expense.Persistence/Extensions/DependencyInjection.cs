using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ExpenseFlow.Expense.Domain.Interfaces;
using ExpenseFlow.Expense.Application.Interfaces;
using ExpenseFlow.Expense.Application.Interfaces.Messaging;
using ExpenseFlow.Expense.Persistence.Repositories;

namespace ExpenseFlow.Expense.Persistence.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ExpenseDb");
        if (string.IsNullOrEmpty(connectionString) || connectionString == "Secret stored in Azure Key Vault")
        {
            connectionString = "Server=localhost;Database=ExpenseFlowExpense;Trusted_Connection=True;TrustServerCertificate=True;";
        }

        services.AddDbContext<ExpenseDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
            options.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        });

        services.AddScoped<DbContext>(provider => provider.GetRequiredService<ExpenseDbContext>());
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ExpenseDbContext>());
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ExpenseDbContext>());

        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services.AddScoped<IExpenseRepository, ExpenseRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        return services;
    }
}
