using System;
using ExpenseFlow.Expense.Domain.Entities;
using ExpenseFlow.Expense.Domain.Interfaces;

namespace ExpenseFlow.Expense.Domain.Interfaces;

public interface IExpenseRepository : IRepository<Entities.Expense, Guid>
{
}
