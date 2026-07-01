using System;
using ExpenseFlow.Expense.Application.Common.Messaging;

namespace ExpenseFlow.Expense.Application.Common.Messaging.Events;

public class ExpenseCreatedIntegrationEvent : BaseIntegrationEvent
{
    public Guid ExpenseId { get; }
    public string Title { get; }
    public decimal Amount { get; }
    public DateTime ExpenseDate { get; }
    public Guid CategoryId { get; }
    public Guid UserId { get; }

    public ExpenseCreatedIntegrationEvent(Guid expenseId, string title, decimal amount, DateTime expenseDate, Guid categoryId, Guid userId)
    {
        ExpenseId = expenseId;
        Title = title;
        Amount = amount;
        ExpenseDate = expenseDate;
        CategoryId = categoryId;
        UserId = userId;
    }
}
