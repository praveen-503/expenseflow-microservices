using System;

namespace ExpenseFlow.Identity.Domain.Entities;

public abstract record EntityId(Guid Value)
{
    public override string ToString() => Value.ToString();
}
