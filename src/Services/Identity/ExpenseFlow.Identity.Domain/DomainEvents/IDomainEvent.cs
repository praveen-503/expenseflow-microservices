using System;

namespace ExpenseFlow.Identity.Domain.DomainEvents;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
