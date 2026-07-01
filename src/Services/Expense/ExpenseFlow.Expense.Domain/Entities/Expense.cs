using System;
using ExpenseFlow.Expense.Domain.Entities;

namespace ExpenseFlow.Expense.Domain.Entities;

public class Expense : AuditableEntity<Guid>
{
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    
    public Guid UserId { get; set; }
}
