using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ExpenseFlow.Expense.Domain.Entities;
using ExpenseFlow.Expense.Domain.Interfaces;
using ExpenseFlow.Expense.Domain.Common;

namespace ExpenseFlow.Expense.Persistence.Repositories;

public class ExpenseRepository : Repository<Domain.Entities.Expense, Guid>, IExpenseRepository
{
    public ExpenseRepository(ExpenseDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> HasExpensesWithCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Domain.Entities.Expense>()
            .AnyAsync(e => e.CategoryId == categoryId, cancellationToken);
    }

    public async Task<ExpenseSummary> GetExpenseSummaryAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var startOfCurrentMonth = new DateTime(now.Year, now.Month, 1);
        var startOfPreviousMonth = startOfCurrentMonth.AddMonths(-1);
        var endOfPreviousMonth = startOfCurrentMonth.AddDays(-1);

        var query = _dbContext.Set<Domain.Entities.Expense>()
            .Where(e => e.UserId == userId);

        var totals = await query
            .GroupBy(e => 1)
            .Select(g => new
            {
                Total = g.Sum(e => e.Amount),
                Max = g.Max(e => e.Amount),
                Min = g.Min(e => e.Amount),
                Avg = g.Average(e => e.Amount)
            })
            .FirstOrDefaultAsync(cancellationToken);

        var currentMonthTotal = await query
            .Where(e => e.ExpenseDate >= startOfCurrentMonth && e.ExpenseDate <= now)
            .SumAsync(e => (decimal?)e.Amount, cancellationToken) ?? 0;

        var previousMonthTotal = await query
            .Where(e => e.ExpenseDate >= startOfPreviousMonth && e.ExpenseDate <= endOfPreviousMonth)
            .SumAsync(e => (decimal?)e.Amount, cancellationToken) ?? 0;

        var expensesByCategory = await query
            .GroupBy(e => e.Category.Name)
            .Select(g => new CategorySummary(g.Key, g.Sum(e => e.Amount)))
            .ToListAsync(cancellationToken);

        return new ExpenseSummary(
            totals?.Total ?? 0,
            expensesByCategory,
            totals?.Max ?? 0,
            totals?.Min ?? 0,
            totals?.Avg ?? 0,
            currentMonthTotal,
            previousMonthTotal
        );
    }
}
