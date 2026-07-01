using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExpenseFlow.Identity.Domain.Entities;
using System;

namespace ExpenseFlow.Identity.Persistence.Configurations;

public class UserConfiguration : BaseEntityConfiguration<User, Guid>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(u => u.LastName).IsRequired().HasMaxLength(100);

        var adminUserId = Guid.Parse("d4c6d678-0df2-46cc-950c-db26c4599c2e");

        builder.HasData(new User
        {
            Id = adminUserId,
            Email = "admin@expenseflow.com",
            // PasswordHash represents "Admin123!" hashed under our PBKDF2 scheme
            PasswordHash = "0102030405060708090A0B0C0D0E0F10-2D7F3389A8C142A9B61E763A75FE013D6B508A56EF240212FA1B6B2F5C5B9904",
            FirstName = "Admin",
            LastName = "System",
            IsActive = true
        });
    }
}
