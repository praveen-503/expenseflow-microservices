using System;

namespace ExpenseFlow.Expense.Application.DTOs;

public record ExpenseDto(
    Guid Id,
    string Title,
    decimal Amount,
    DateTime ExpenseDate,
    string Notes,
    Guid CategoryId,
    CategoryDto Category,
    Guid UserId,
    DateTime CreatedAt,
    DateTime? LastModifiedAt);
