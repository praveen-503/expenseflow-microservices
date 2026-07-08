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

public class ExpenseDeletedIntegrationEventConsumer : IEventConsumer<ExpenseDeletedIntegrationEvent>
{
    private readonly ISender _mediator;
    private readonly ILogger<ExpenseDeletedIntegrationEventConsumer> _logger;

    public ExpenseDeletedIntegrationEventConsumer(ISender mediator, ILogger<ExpenseDeletedIntegrationEventConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task ConsumeAsync(ExpenseDeletedIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Consuming ExpenseDeletedIntegrationEvent for Expense: {ExpenseId}", @event.ExpenseId);

        var message = "An expense has been removed from your account.";
        var command = new SendNotificationCommand(
            @event.UserId,
            "Expense Deleted",
            message,
            NotificationType.Email);

        await _mediator.Send(command, cancellationToken);
    }
}
