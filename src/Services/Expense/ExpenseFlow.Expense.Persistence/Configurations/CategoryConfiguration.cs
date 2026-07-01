using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExpenseFlow.Expense.Domain.Entities;

namespace ExpenseFlow.Expense.Persistence.Configurations;

public class CategoryConfiguration : BaseEntityConfiguration<Category, Guid>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);

        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Description).HasMaxLength(500);
    }
}
