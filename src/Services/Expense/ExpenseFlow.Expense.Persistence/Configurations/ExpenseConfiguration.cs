using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExpenseFlow.Expense.Domain.Entities;
using System;

namespace ExpenseFlow.Expense.Persistence.Configurations;

public class ExpenseConfiguration : BaseEntityConfiguration<Domain.Entities.Expense, Guid>
{
    public override void Configure(EntityTypeBuilder<Domain.Entities.Expense> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.Title).IsRequired().HasMaxLength(150);
        builder.Property(e => e.Amount).HasColumnType("decimal(18,2)");
        builder.Property(e => e.Notes).HasMaxLength(500);

        builder.HasOne(e => e.Category)
            .WithMany(c => c.Expenses)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

