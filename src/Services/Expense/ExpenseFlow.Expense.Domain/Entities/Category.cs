using System;
using System.Collections.Generic;

namespace ExpenseFlow.Expense.Domain.Entities;

public class Category : BaseEntity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}
