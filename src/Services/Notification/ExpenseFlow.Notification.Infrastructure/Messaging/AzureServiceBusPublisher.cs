using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using ExpenseFlow.Notification.Application.Interfaces.Messaging;
using ExpenseFlow.Notification.Application.Common.Messaging;

namespace ExpenseFlow.Notification.Infrastructure.Messaging;

public class AzureServiceBusPublisher : IEventPublisher
{
    private readonly ILogger<AzureServiceBusPublisher> _logger;

    public AzureServiceBusPublisher(ILogger<AzureServiceBusPublisher> logger)
    {
        _logger = logger;
    }

    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) 
        where TEvent : BaseIntegrationEvent
    {
        _logger.LogInformation("Azure Service Bus [Abstraction]: Publishing Integration Event {EventId} of type {EventType}", 
            @event.Id, typeof(TEvent).Name);

        // Actual client implementation with Azure Service Bus will be placed here
        return Task.CompletedTask;
    }
}
