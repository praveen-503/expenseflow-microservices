using MediatR;
using System;

namespace ExpenseFlow.Identity.Domain.DomainEvents;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
