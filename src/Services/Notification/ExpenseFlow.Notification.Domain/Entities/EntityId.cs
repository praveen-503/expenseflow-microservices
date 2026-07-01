using System;

namespace ExpenseFlow.Notification.Domain.Entities;

public abstract record EntityId(Guid Value)
{
    public override string ToString() => Value.ToString();
}
