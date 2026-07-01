using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ExpenseFlow.Expense.Domain.Entities;
using ExpenseFlow.Expense.Domain.Interfaces;

namespace ExpenseFlow.Expense.Persistence.Repositories;

public class CategoryRepository : Repository<Category, Guid>, ICategoryRepository
{
    public CategoryRepository(ExpenseDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Category?> GetByNameAndUserAsync(string name, Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Category>()
            .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower() && (c.UserId == userId || c.UserId == null), cancellationToken);
    }

    public async Task<IReadOnlyList<Category>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Category>()
            .Where(c => c.UserId == userId || c.UserId == null)
            .ToListAsync(cancellationToken);
    }
}
