using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using ExpenseFlow.Notification.Application.Interfaces.Messaging;
using ExpenseFlow.Notification.Application.Common.Messaging;

namespace ExpenseFlow.Notification.Infrastructure.Messaging;

public class AzureServiceBusConsumerService : BackgroundService
{
    private readonly ILogger<AzureServiceBusConsumerService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ServiceBusClient _client;
    private readonly ServiceBusOptions _options;
    private ServiceBusProcessor? _processor;

    private static readonly Dictionary<string, Type> EventTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        { nameof(UserRegisteredIntegrationEvent), typeof(UserRegisteredIntegrationEvent) },
        { nameof(ExpenseCreatedIntegrationEvent), typeof(ExpenseCreatedIntegrationEvent) },
        { nameof(ExpenseUpdatedIntegrationEvent), typeof(ExpenseUpdatedIntegrationEvent) },
        { nameof(ExpenseDeletedIntegrationEvent), typeof(ExpenseDeletedIntegrationEvent) }
    };

    public AzureServiceBusConsumerService(
        ILogger<AzureServiceBusConsumerService> logger, 
        IServiceScopeFactory scopeFactory,
        ServiceBusClient client,
        IOptions<ServiceBusOptions> options)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _client = client;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Azure Service Bus [Consumer]: Initializing listener background task...");

        // 1. Setup Administration Client to auto-create resources if possible
        await EnsureResourcesExistAsync(stoppingToken);

        // 2. Initialize ServiceBusProcessor
        var processorOptions = new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = false,
            MaxConcurrentCalls = 1
        };

        _processor = _client.CreateProcessor(_options.TopicName, _options.SubscriptionName, processorOptions);

        _processor.ProcessMessageAsync += MessageHandler;
        _processor.ProcessErrorAsync += ErrorHandler;

        _logger.LogInformation("Azure Service Bus [Consumer]: Starting processor listening on Topic '{TopicName}' and Subscription '{SubscriptionName}'...", 
            _options.TopicName, _options.SubscriptionName);

        await _processor.StartProcessingAsync(stoppingToken);

        // Keep running until cancellation requested
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(5000, stoppingToken);
        }

        _logger.LogInformation("Azure Service Bus [Consumer]: Stopping processor...");
        await _processor.StopProcessingAsync(CancellationToken.None);
        
        _processor.ProcessMessageAsync -= MessageHandler;
        _processor.ProcessErrorAsync -= ErrorHandler;

        await _processor.DisposeAsync();
    }

    private async Task EnsureResourcesExistAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Attempting to auto-provision Service Bus Topic '{TopicName}' and Subscription '{SubscriptionName}'...", 
                _options.TopicName, _options.SubscriptionName);

            var adminClient = new ServiceBusAdministrationClient(_options.ConnectionString);

            if (!await adminClient.TopicExistsAsync(_options.TopicName, cancellationToken))
            {
                await adminClient.CreateTopicAsync(_options.TopicName, cancellationToken);
                _logger.LogInformation("Topic '{TopicName}' was successfully created.", _options.TopicName);
            }

            if (!string.IsNullOrEmpty(_options.SubscriptionName))
            {
                if (!await adminClient.SubscriptionExistsAsync(_options.TopicName, _options.SubscriptionName, cancellationToken))
                {
                    await adminClient.CreateSubscriptionAsync(_options.TopicName, _options.SubscriptionName, cancellationToken);
                    _logger.LogInformation("Subscription '{SubscriptionName}' under Topic '{TopicName}' was successfully created.", 
                        _options.SubscriptionName, _options.TopicName);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to auto-provision Service Bus resources. Assuming they exist and proceeding. Details: {Message}", ex.Message);
        }
    }

    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        var message = args.Message;
        var messageBody = message.Body.ToString();
        
        // Find MessageType from properties or subject
        if (!message.ApplicationProperties.TryGetValue("MessageType", out var messageTypeObj) || 
            messageTypeObj is not string messageType)
        {
            messageType = message.Subject;
        }

        _logger.LogInformation("Received message {MessageId} (DeliveryCount: {DeliveryCount}) with MessageType '{MessageType}'.", 
            message.MessageId, message.DeliveryCount, messageType);

        if (string.IsNullOrEmpty(messageType) || !EventTypes.TryGetValue(messageType, out var eventType))
        {
            _logger.LogWarning("Message {MessageId} contains unknown message type '{MessageType}'. Sending directly to Dead Letter Queue.", 
                message.MessageId, messageType);
            
            await args.DeadLetterMessageAsync(message, "UnrecognizedMessageType", $"The message type '{messageType}' is not supported by this consumer.");
            return;
        }

        object? eventData;
        try
        {
            eventData = JsonSerializer.Deserialize(messageBody, eventType);
            if (eventData == null)
            {
                throw new JsonException("Deserialized payload is null.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to deserialize message {MessageId} payload into type '{MessageType}'. Sending directly to Dead Letter Queue.", 
                message.MessageId, messageType);
            
            await args.DeadLetterMessageAsync(message, "DeserializationError", ex.Message);
            return;
        }

        // Process message via consumer
        using (var scope = _scopeFactory.CreateScope())
        {
            var consumerType = typeof(IEventConsumer<>).MakeGenericType(eventType);
            var consumer = scope.ServiceProvider.GetService(consumerType);

            if (consumer == null)
            {
                _logger.LogWarning("No consumer handler registered in DI for event type '{MessageType}'. Dead-lettering message.", messageType);
                await args.DeadLetterMessageAsync(message, "NoConsumerRegistered", $"IEventConsumer<{messageType}> is not registered in the service provider.");
                return;
            }

            try
            {
                _logger.LogInformation("Invoking consumer handler {ConsumerType} for message {MessageId}...", 
                    consumer.GetType().Name, message.MessageId);

                var consumeMethod = consumerType.GetMethod(nameof(IEventConsumer<BaseIntegrationEvent>.ConsumeAsync));
                if (consumeMethod == null)
                {
                    throw new InvalidOperationException($"Method 'ConsumeAsync' not found on consumer type {consumerType.FullName}.");
                }

                await (Task)consumeMethod.Invoke(consumer, new[] { eventData, args.CancellationToken })!;

                _logger.LogInformation("Successfully processed message {MessageId} (MessageType: '{MessageType}'). Completing message...", 
                    message.MessageId, messageType);

                await args.CompleteMessageAsync(message);
            }
            catch (Exception ex)
            {
                // Retrieve inner exception if invoked method failed
                var actualException = ex.InnerException ?? ex;

                _logger.LogError(actualException, "Error occurred while executing consumer handler for message {MessageId} of type '{MessageType}'. Release lock for retry.", 
                    message.MessageId, messageType);

                // Throwing exception lets Azure Service Bus handle retry and move it to DLQ once MaxDeliveryCount is reached.
                throw;
            }
        }
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception, "Azure Service Bus processor error occurred! Source: {ErrorSource}, Namespace: {FullyQualifiedNamespace}, EntityPath: {EntityPath}", 
            args.ErrorSource, args.FullyQualifiedNamespace, args.EntityPath);

        return Task.CompletedTask;
    }
}
