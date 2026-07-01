using System;
using ExpenseFlow.Expense.Domain.Entities;
using ExpenseFlow.Expense.Domain.Specifications;

namespace ExpenseFlow.Expense.Domain.Specifications;

public class ExpenseWithCategorySpecification : Specification<Entities.Expense>
{
    public ExpenseWithCategorySpecification(Guid userId) 
        : base(e => e.UserId == userId)
    {
        AddInclude(e => e.Category);
    }
}
