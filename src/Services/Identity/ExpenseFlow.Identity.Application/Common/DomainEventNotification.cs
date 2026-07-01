using MediatR;
using ExpenseFlow.Identity.Domain.DomainEvents;

namespace ExpenseFlow.Identity.Application.Common;

public class DomainEventNotification<TEvent> : INotification where TEvent : IDomainEvent
{
    public DomainEventNotification(TEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }

    public TEvent DomainEvent { get; }
}
