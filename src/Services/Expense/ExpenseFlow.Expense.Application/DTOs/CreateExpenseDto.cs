using System;

namespace ExpenseFlow.Expense.Application.DTOs;

public record CreateExpenseDto(string Description, decimal Amount, DateTime Date, Guid CategoryId);
