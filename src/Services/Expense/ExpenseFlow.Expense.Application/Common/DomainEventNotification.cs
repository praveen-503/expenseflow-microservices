using MediatR;
using ExpenseFlow.Expense.Domain.DomainEvents;

namespace ExpenseFlow.Expense.Application.Common;

public class DomainEventNotification<TEvent> : INotification where TEvent : IDomainEvent
{
    public DomainEventNotification(TEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }

    public TEvent DomainEvent { get; }
}
