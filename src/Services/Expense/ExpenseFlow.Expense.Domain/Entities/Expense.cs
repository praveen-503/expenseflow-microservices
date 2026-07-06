using System;

namespace ExpenseFlow.Expense.Domain.Entities;

public class Expense : AuditableEntity<Guid>
{
    public string Title { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime ExpenseDate { get; set; }
    public string Notes { get; set; } = string.Empty;
    
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    
    public Guid UserId { get; set; }
    
    public string? ReceiptUrl { get; set; }
}
