using System;

namespace ExpenseFlow.Expense.Application.DTOs;

public record CategoryDto(Guid Id, string Name, string Description);
