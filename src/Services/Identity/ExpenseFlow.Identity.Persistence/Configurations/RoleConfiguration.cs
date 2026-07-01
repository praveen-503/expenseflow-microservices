using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExpenseFlow.Identity.Domain.Entities;
using System;

namespace ExpenseFlow.Identity.Persistence.Configurations;

public class RoleConfiguration : BaseEntityConfiguration<Role, Guid>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);

        builder.Property(r => r.Name).IsRequired().HasMaxLength(100);

        var adminRoleId = Guid.Parse("a5c9a9f9-e3b9-4a0d-85f0-8c29db26c459");
        var userRoleId = Guid.Parse("f6b0f0a0-0df1-4a11-a590-db26c4599c2d");

        builder.HasData(
            new Role { Id = adminRoleId, Name = "Administrator" },
            new Role { Id = userRoleId, Name = "User" }
        );
    }
}
