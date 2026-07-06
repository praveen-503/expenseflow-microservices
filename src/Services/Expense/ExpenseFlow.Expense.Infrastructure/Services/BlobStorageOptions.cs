namespace ExpenseFlow.Expense.Infrastructure.Services;

public class BlobStorageOptions
{
    public const string SectionName = "BlobStorage";

    public string ConnectionString { get; set; } = string.Empty;
    public string FullyQualifiedNamespace { get; set; } = string.Empty;
    public string ContainerName { get; set; } = "receipts";
}
