using System;

namespace ExpenseFlow.Expense.Domain.DomainEvents;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
