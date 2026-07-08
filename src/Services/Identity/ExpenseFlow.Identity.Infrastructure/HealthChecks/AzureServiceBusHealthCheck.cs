using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using ExpenseFlow.Identity.Infrastructure.Messaging;

namespace ExpenseFlow.Identity.Infrastructure.HealthChecks;

public class AzureServiceBusHealthCheck : IHealthCheck
{
    private readonly ServiceBusClient _client;
    private readonly string _topicName;

    public AzureServiceBusHealthCheck(ServiceBusClient client, IOptions<ServiceBusOptions> options)
    {
        _client = client;
        _topicName = options.Value.TopicName;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var sender = _client.CreateSender(_topicName);
            // Creating a message batch contacts the Service Bus namespace and checks if credentials are valid
            var batch = await sender.CreateMessageBatchAsync(cancellationToken);
            return HealthCheckResult.Healthy($"Successfully connected to Azure Service Bus namespace. Topic '{_topicName}' is accessible.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Failed connection check to Azure Service Bus namespace.", ex);
        }
    }
}
