using MediatR;
using System;

namespace ExpenseFlow.Notification.Domain.DomainEvents;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
