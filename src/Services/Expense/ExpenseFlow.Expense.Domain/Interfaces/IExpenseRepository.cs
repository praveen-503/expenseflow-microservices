using System;
using System.Threading;
using System.Threading.Tasks;
using ExpenseFlow.Expense.Domain.Entities;
using ExpenseFlow.Expense.Domain.Interfaces;
using ExpenseFlow.Expense.Domain.Common;

namespace ExpenseFlow.Expense.Domain.Interfaces;

public interface IExpenseRepository : IRepository<Entities.Expense, Guid>
{
    Task<bool> HasExpensesWithCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<ExpenseSummary> GetExpenseSummaryAsync(Guid userId, CancellationToken cancellationToken = default);
}
