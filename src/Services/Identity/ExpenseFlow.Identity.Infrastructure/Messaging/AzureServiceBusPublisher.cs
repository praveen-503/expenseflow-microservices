using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ExpenseFlow.Identity.Application.Interfaces.Messaging;
using ExpenseFlow.Identity.Application.Common.Messaging;

namespace ExpenseFlow.Identity.Infrastructure.Messaging;

public class AzureServiceBusPublisher : IEventPublisher, IAsyncDisposable
{
    private readonly ServiceBusSender _sender;
    private readonly ILogger<AzureServiceBusPublisher> _logger;
    private readonly ServiceBusOptions _options;

    public AzureServiceBusPublisher(
        ServiceBusClient client, 
        IOptions<ServiceBusOptions> options, 
        ILogger<AzureServiceBusPublisher> logger)
    {
        _options = options.Value;
        _sender = client.CreateSender(_options.TopicName);
        _logger = logger;
    }

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) 
        where TEvent : BaseIntegrationEvent
    {
        try
        {
            var eventId = @event.Id;
            var eventType = typeof(TEvent).Name;
            var payloadJson = JsonSerializer.Serialize(@event);
            
            var message = new ServiceBusMessage(payloadJson)
            {
                MessageId = eventId.ToString(),
                Subject = eventType
            };
            message.ApplicationProperties["MessageType"] = eventType;

            _logger.LogInformation("Publishing event {EventId} of type {EventType} to topic {TopicName}...", 
                eventId, eventType, _options.TopicName);

            await _sender.SendMessageAsync(message, cancellationToken);

            _logger.LogInformation("Successfully published event {EventId}.", eventId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish event {@Event} to Azure Service Bus topic {TopicName}.", 
                @event, _options.TopicName);
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _sender.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
