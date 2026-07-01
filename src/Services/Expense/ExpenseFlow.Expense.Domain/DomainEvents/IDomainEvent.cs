using MediatR;
using System;

namespace ExpenseFlow.Expense.Domain.DomainEvents;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
