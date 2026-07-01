using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using ExpenseFlow.Notification.Application.Interfaces.Messaging;
using ExpenseFlow.Notification.Application.Common.Messaging;
using System;

namespace ExpenseFlow.Notification.Infrastructure.Messaging;

public class AzureServiceBusConsumerService : BackgroundService
{
    private readonly ILogger<AzureServiceBusConsumerService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public AzureServiceBusConsumerService(ILogger<AzureServiceBusConsumerService> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Azure Service Bus [Consumer Abstraction]: Starting listener background task...");

        // Design: In a real environment connected to Azure Service Bus:
        // 1. Initialize ServiceBusClient and ServiceBusProcessor for the topics/queues.
        // 2. Register message and error handler callbacks.
        // 3. In the message handler callback, deserialized payload and resolve consumer:
        //    using (var scope = _scopeFactory.CreateScope()) {
        //        var consumer = scope.ServiceProvider.GetRequiredService<IEventConsumer<TEvent>>();
        //        await consumer.ConsumeAsync(eventData);
        //    }
        // 4. Start processing asynchronously.

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogDebug("Azure Service Bus [Consumer Abstraction]: Listening for incoming microservice events...");
            await Task.Delay(15000, stoppingToken); // Check-in log every 15 seconds
        }

        _logger.LogInformation("Azure Service Bus [Consumer Abstraction]: Stopping listener background task...");
    }
}
