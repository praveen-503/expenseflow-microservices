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

public class UserRegisteredIntegrationEventConsumer : IEventConsumer<UserRegisteredIntegrationEvent>
{
    private readonly ISender _mediator;
    private readonly ILogger<UserRegisteredIntegrationEventConsumer> _logger;

    public UserRegisteredIntegrationEventConsumer(ISender mediator, ILogger<UserRegisteredIntegrationEventConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task ConsumeAsync(UserRegisteredIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Consuming UserRegisteredIntegrationEvent for user: {Email}", @event.Email);

        var welcomeMessage = $"Welcome {@event.FirstName}! We are thrilled to welcome you to ExpenseFlow.";
        var command = new SendNotificationCommand(
            @event.UserId,
            "Welcome to ExpenseFlow!",
            welcomeMessage,
            NotificationType.Email);

        await _mediator.Send(command, cancellationToken);
    }
}
