using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MediatR;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using ExpenseFlow.Notification.Application.Commands;
using ExpenseFlow.Notification.Application.Common.Messaging;
using ExpenseFlow.Notification.Domain.Enums;

namespace ExpenseFlow.Notification.Functions;

public class ExpenseCreatedFunction
{
    private readonly ISender _mediator;
    private readonly ILogger<ExpenseCreatedFunction> _logger;

    public ExpenseCreatedFunction(ISender mediator, ILogger<ExpenseCreatedFunction> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [Function("ExpenseCreatedFunction")]
    public async Task Run(
        [ServiceBusTrigger(
            "%ServiceBus:TopicName%", 
            "%ServiceBus:SubscriptionName%", 
            Connection = "ServiceBusConnection")] string messageBody,
        FunctionContext context)
    {
        _logger.LogInformation("Azure Function [ServiceBusTrigger]: Processing message received. Message ID: {MessageId}", context.InvocationId);

        ExpenseCreatedIntegrationEvent? @event;
        try
        {
            @event = JsonSerializer.Deserialize<ExpenseCreatedIntegrationEvent>(messageBody);
            if (@event == null)
            {
                throw new JsonException("Deserialized integration event is null.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to deserialize integration event payload. Error: {Message}", ex.Message);
            // Non-transient deserialization error; complete message to avoid infinite retries
            return;
        }

        _logger.LogInformation("Processing ExpenseCreatedIntegrationEvent for Expense: {ExpenseId}, User: {UserId}", @event.ExpenseId, @event.UserId);

        var message = $"A new expense of {@event.Amount:C} for '{@event.Title}' was added to your account.";
        var command = new SendNotificationCommand(
            @event.UserId,
            "New Expense Logged",
            message,
            NotificationType.Email);

        var result = await _mediator.Send(command);
        if (result.IsFailure)
        {
            _logger.LogError("MediatR Command SendNotificationCommand failed. Error Code: {ErrorCode}, Message: {ErrorMessage}", 
                result.Error.Code, result.Error.Message);
            
            // Throw exception to trigger FixedDelayRetry policy
            throw new InvalidOperationException($"Notification delivery failed: {result.Error.Message}");
        }

        _logger.LogInformation("Successfully processed ExpenseCreatedIntegrationEvent for Expense: {ExpenseId}", @event.ExpenseId);
    }
}
