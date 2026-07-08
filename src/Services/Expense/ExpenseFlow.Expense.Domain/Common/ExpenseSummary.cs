using System.Collections.Generic;

namespace ExpenseFlow.Expense.Domain.Common;

public class ExpenseSummary
{
    public decimal TotalExpense { get; }
    public IReadOnlyList<CategorySummary> ExpensesByCategory { get; }
    public decimal HighestExpense { get; }
    public decimal LowestExpense { get; }
    public decimal AverageExpense { get; }
    public decimal CurrentMonthTotal { get; }
    public decimal PreviousMonthTotal { get; }

    public ExpenseSummary(
        decimal totalExpense,
        IReadOnlyList<CategorySummary> expensesByCategory,
        decimal highestExpense,
        decimal lowestExpense,
        decimal averageExpense,
        decimal currentMonthTotal,
        decimal previousMonthTotal)
    {
        TotalExpense = totalExpense;
        ExpensesByCategory = expensesByCategory;
        HighestExpense = highestExpense;
        LowestExpense = lowestExpense;
        AverageExpense = averageExpense;
        CurrentMonthTotal = currentMonthTotal;
        PreviousMonthTotal = previousMonthTotal;
    }
}
