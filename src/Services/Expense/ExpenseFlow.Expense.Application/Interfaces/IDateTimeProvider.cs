using System;

namespace ExpenseFlow.Expense.Application.Interfaces;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
