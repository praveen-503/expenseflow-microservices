namespace ExpenseFlow.Notification.Infrastructure.Messaging;

public class ServiceBusOptions
{
    public const string SectionName = "ServiceBus";

    public string ConnectionString { get; set; } = string.Empty;
    public string FullyQualifiedNamespace { get; set; } = string.Empty;
    public string TopicName { get; set; } = "expenseflow-integration-events";
    public string SubscriptionName { get; set; } = string.Empty;
    public RetryOptions Retry { get; set; } = new();
}

public class RetryOptions
{
    public int MaxRetries { get; set; } = 3;
    public double DelaySeconds { get; set; } = 2.0;
    public double MaxDelaySeconds { get; set; } = 10.0;
    public string Mode { get; set; } = "Exponential"; // Exponential or Fixed
}
