using System.Collections.Generic;
using ExpenseFlow.Notification.Domain.DomainEvents;

namespace ExpenseFlow.Notification.Domain.Entities;

public interface IBaseEntity
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}
