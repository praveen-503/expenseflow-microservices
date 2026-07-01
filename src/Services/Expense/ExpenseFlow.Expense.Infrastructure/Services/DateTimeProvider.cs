using System;
using ExpenseFlow.Expense.Application.Interfaces;

namespace ExpenseFlow.Expense.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
