using System;

namespace ExpenseFlow.Identity.Application.Common.Messaging;

public abstract class BaseIntegrationEvent
{
    protected BaseIntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }

    public Guid Id { get; }
    public DateTime CreationDate { get; }
}
