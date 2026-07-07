using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ExpenseFlow.Notification.Domain.Interfaces;
using ExpenseFlow.Notification.Application.Interfaces;
using ExpenseFlow.Notification.Application.Interfaces.Messaging;
using ExpenseFlow.Notification.Persistence.Repositories;

namespace ExpenseFlow.Notification.Persistence.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("NotificationDb");
        if (string.IsNullOrEmpty(connectionString) || connectionString == "Secret stored in Azure Key Vault")
        {
            connectionString = "Server=localhost;Database=ExpenseFlowNotification;Trusted_Connection=True;TrustServerCertificate=True;";
        }

        services.AddDbContext<NotificationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
            options.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<NotificationDbContext>());
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<NotificationDbContext>());

        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        return services;
    }
}
