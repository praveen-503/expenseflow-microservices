using System;
using ExpenseFlow.Expense.Application.Common.Messaging;

namespace ExpenseFlow.Expense.Application.Common.Messaging.Events;

public class ExpenseDeletedIntegrationEvent : BaseIntegrationEvent
{
    public Guid ExpenseId { get; }
    public Guid UserId { get; }

    public ExpenseDeletedIntegrationEvent(Guid expenseId, Guid userId)
    {
        ExpenseId = expenseId;
        UserId = userId;
    }
}
