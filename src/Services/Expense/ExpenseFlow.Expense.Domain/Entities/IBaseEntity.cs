using System.Collections.Generic;
using ExpenseFlow.Expense.Domain.DomainEvents;

namespace ExpenseFlow.Expense.Domain.Entities;

public interface IBaseEntity
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}
