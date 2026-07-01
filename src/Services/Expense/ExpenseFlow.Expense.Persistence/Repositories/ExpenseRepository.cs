using System;
using ExpenseFlow.Expense.Domain.Entities;
using ExpenseFlow.Expense.Domain.Interfaces;

namespace ExpenseFlow.Expense.Persistence.Repositories;

public class ExpenseRepository : Repository<Domain.Entities.Expense, Guid>, IExpenseRepository
{
    public ExpenseRepository(ExpenseDbContext dbContext) : base(dbContext)
    {
    }
}
