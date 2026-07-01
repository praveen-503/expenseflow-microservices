using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ExpenseFlow.Expense.Domain.Entities;
using ExpenseFlow.Expense.Domain.Interfaces;

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
}
