using System;
using ExpenseFlow.Expense.Domain.Entities;
using ExpenseFlow.Expense.Domain.Interfaces;

namespace ExpenseFlow.Expense.Domain.Interfaces;

public interface ICategoryRepository : IRepository<Category, Guid>
{
}
