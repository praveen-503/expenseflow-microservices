using System;

namespace ExpenseFlow.Expense.Domain.DomainEvents;

public abstract class BaseDomainEvent : IDomainEvent
{
    protected BaseDomainEvent()
    {
        Id = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }

    public Guid Id { get; }
    public DateTime OccurredOn { get; }
}
