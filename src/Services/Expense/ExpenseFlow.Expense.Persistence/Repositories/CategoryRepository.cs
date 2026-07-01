using System;
using ExpenseFlow.Expense.Domain.Entities;
using ExpenseFlow.Expense.Domain.Interfaces;

namespace ExpenseFlow.Expense.Persistence.Repositories;

public class CategoryRepository : Repository<Category, Guid>, ICategoryRepository
{
    public CategoryRepository(ExpenseDbContext dbContext) : base(dbContext)
    {
    }
}
