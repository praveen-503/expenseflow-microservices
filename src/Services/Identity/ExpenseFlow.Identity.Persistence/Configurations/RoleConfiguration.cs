using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExpenseFlow.Identity.Domain.Entities;

namespace ExpenseFlow.Identity.Persistence.Configurations;

public class RoleConfiguration : BaseEntityConfiguration<Role, Guid>
{
    public override void Configure(EntityTypeBuilder<Role> builder)
    {
        base.Configure(builder);

        builder.Property(r => r.Name).IsRequired().HasMaxLength(100);
    }
}
