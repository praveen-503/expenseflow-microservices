namespace ExpenseFlow.Expense.Domain.Common;

public class CategorySummary
{
    public string CategoryName { get; }
    public decimal TotalAmount { get; }

    public CategorySummary(string categoryName, decimal totalAmount)
    {
        CategoryName = categoryName;
        TotalAmount = totalAmount;
    }
}
