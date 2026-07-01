using System;

namespace ExpenseFlow.Expense.Application.DTOs;

public record ExpenseDto(Guid Id, string Description, decimal Amount, DateTime Date, Guid CategoryId, CategoryDto Category, Guid UserId);
