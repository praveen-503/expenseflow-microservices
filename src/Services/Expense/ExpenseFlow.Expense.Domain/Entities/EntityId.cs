using System;

namespace ExpenseFlow.Expense.Domain.Entities;

public abstract record EntityId(Guid Value)
{
    public override string ToString() => Value.ToString();
}
