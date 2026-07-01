using System;
using ExpenseFlow.Expense.Domain.Entities;
using ExpenseFlow.Expense.Domain.Specifications;

namespace ExpenseFlow.Expense.Domain.Specifications;

public class ExpenseByIdWithCategorySpecification : Specification<Entities.Expense>
{
    public ExpenseByIdWithCategorySpecification(Guid id, Guid userId) 
        : base(e => e.Id == id && e.UserId == userId)
    {
        AddInclude(e => e.Category);
    }
}
