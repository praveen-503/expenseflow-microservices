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

public class ExpenseUpdatedIntegrationEventConsumer : IEventConsumer<ExpenseUpdatedIntegrationEvent>
{
    private readonly ISender _mediator;
    private readonly ILogger<ExpenseUpdatedIntegrationEventConsumer> _logger;

    public ExpenseUpdatedIntegrationEventConsumer(ISender mediator, ILogger<ExpenseUpdatedIntegrationEventConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task ConsumeAsync(ExpenseUpdatedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Consuming ExpenseUpdatedIntegrationEvent for Expense: {ExpenseId}", @event.ExpenseId);

        var message = $"Your expense '{@event.Title}' has been updated. The new amount is {@event.Amount:C}.";
        var command = new SendNotificationCommand(
            @event.UserId,
            "Expense Updated",
            message,
            NotificationType.Email);

        await _mediator.Send(command, cancellationToken);
    }
}
