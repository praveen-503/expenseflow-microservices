using System;

namespace ExpenseFlow.Notification.Domain.DomainEvents;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
