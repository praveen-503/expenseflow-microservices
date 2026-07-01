using System.Collections.Generic;

namespace ExpenseFlow.Expense.Application.DTOs;

public record CategorySummaryDto(string CategoryName, decimal TotalAmount);

public record ExpenseSummaryDto(
    decimal TotalExpense,
    List<CategorySummaryDto> ExpensesByCategory,
    decimal HighestExpense,
    decimal LowestExpense,
    decimal AverageExpense,
    decimal CurrentMonthTotal,
    decimal PreviousMonthTotal);
