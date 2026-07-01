using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExpenseFlow.Expense.Domain.Entities;
using ExpenseFlow.Expense.Domain.Interfaces;

namespace ExpenseFlow.Expense.Domain.Interfaces;

public interface ICategoryRepository : IRepository<Category, Guid>
{
    Task<Category?> GetByNameAndUserAsync(string name, Guid userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Category>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
