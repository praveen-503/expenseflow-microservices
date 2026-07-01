using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExpenseFlow.Identity.Domain.Entities;
using System;

namespace ExpenseFlow.Identity.Persistence.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(ur => new { ur.UserId, ur.RoleId });

        builder.HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        builder.HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        var adminRoleId = Guid.Parse("a5c9a9f9-e3b9-4a0d-85f0-8c29db26c459");
        var adminUserId = Guid.Parse("d4c6d678-0df2-46cc-950c-db26c4599c2e");

        builder.HasData(new UserRole { UserId = adminUserId, RoleId = adminRoleId });
    }
}
