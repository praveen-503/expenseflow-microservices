using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ExpenseFlow.Notification.Application.Interfaces.Messaging;
using ExpenseFlow.Notification.Application.Common.Messaging;
using ExpenseFlow.Notification.Domain.Enums;
using ExpenseFlow.Notification.Application.Commands;
using MediatR;

namespace ExpenseFlow.Notification.Application.Handlers;

public class ExpenseCreatedIntegrationEventConsumer : IEventConsumer<ExpenseCreatedIntegrationEvent>
{
    private readonly ISender _mediator;
    private readonly ILogger<ExpenseCreatedIntegrationEventConsumer> _logger;

    public ExpenseCreatedIntegrationEventConsumer(ISender mediator, ILogger<ExpenseCreatedIntegrationEventConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task ConsumeAsync(ExpenseCreatedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Consuming ExpenseCreatedIntegrationEvent for Expense: {ExpenseId}", @event.ExpenseId);

        var message = $"A new expense of {@event.Amount:C} for '{@event.Title}' was added to your account.";
        var command = new SendNotificationCommand(
            @event.UserId,
            "New Expense Logged",
            message,
            NotificationType.Email);

        await _mediator.Send(command, cancellationToken);
    }
}
