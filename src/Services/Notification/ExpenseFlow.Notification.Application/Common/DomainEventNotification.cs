using MediatR;
using ExpenseFlow.Notification.Domain.DomainEvents;

namespace ExpenseFlow.Notification.Application.Common;

public class DomainEventNotification<TEvent> : INotification where TEvent : IDomainEvent
{
    public DomainEventNotification(TEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }

    public TEvent DomainEvent { get; }
}
