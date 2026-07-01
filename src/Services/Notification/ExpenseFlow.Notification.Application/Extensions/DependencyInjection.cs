using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using FluentValidation;
using ExpenseFlow.Notification.Application.Behaviors;
using ExpenseFlow.Notification.Application.Interfaces.Messaging;
using ExpenseFlow.Notification.Application.Common.Messaging;
using ExpenseFlow.Notification.Application.Handlers;

namespace ExpenseFlow.Notification.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        });

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Register Integration Event Consumers
        services.AddScoped<IEventConsumer<UserRegisteredIntegrationEvent>, UserRegisteredIntegrationEventConsumer>();
        services.AddScoped<IEventConsumer<ExpenseCreatedIntegrationEvent>, ExpenseCreatedIntegrationEventConsumer>();
        services.AddScoped<IEventConsumer<ExpenseUpdatedIntegrationEvent>, ExpenseUpdatedIntegrationEventConsumer>();
        services.AddScoped<IEventConsumer<ExpenseDeletedIntegrationEvent>, ExpenseDeletedIntegrationEventConsumer>();

        return services;
    }
}
