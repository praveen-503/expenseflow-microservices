using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExpenseFlow.Expense.Domain.Entities;
using System;

namespace ExpenseFlow.Expense.Persistence.Configurations;

public class CategoryConfiguration : BaseEntityConfiguration<Category, Guid>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);

        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Description).HasMaxLength(500);

        builder.HasData(
            new Category { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Food & Dining", Description = "Meals, groceries, and dining out" },
            new Category { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Transportation", Description = "Fuel, public transit, and vehicle maintenance" },
            new Category { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Utilities", Description = "Electricity, water, gas, and internet" },
            new Category { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "Entertainment", Description = "Movies, games, events, and hobbies" },
            new Category { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Name = "Housing", Description = "Rent, mortgage, and home insurance" }
        );
    }
}
