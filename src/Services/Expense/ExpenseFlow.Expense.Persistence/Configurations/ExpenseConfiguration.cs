using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExpenseFlow.Expense.Domain.Entities;

namespace ExpenseFlow.Expense.Persistence.Configurations;

public class ExpenseConfiguration : BaseEntityConfiguration<Domain.Entities.Expense, Guid>
{
    public override void Configure(EntityTypeBuilder<Domain.Entities.Expense> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.Description).IsRequired().HasMaxLength(500);
        builder.Property(e => e.Amount).IsRequired().HasPrecision(18, 2);
        builder.Property(e => e.Date).IsRequired();
        builder.Property(e => e.UserId).IsRequired();

        builder.HasOne(e => e.Category)
            .WithMany(c => c.Expenses)
            .HasForeignKey(e => e.CategoryId);
    }
}
