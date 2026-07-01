using System.Collections.Generic;
using ExpenseFlow.Identity.Domain.DomainEvents;

namespace ExpenseFlow.Identity.Domain.Entities;

public interface IBaseEntity
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}
