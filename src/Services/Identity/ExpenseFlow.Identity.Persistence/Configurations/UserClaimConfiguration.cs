using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExpenseFlow.Identity.Domain.Entities;

namespace ExpenseFlow.Identity.Persistence.Configurations;

public class UserClaimConfiguration : BaseEntityConfiguration<UserClaim, Guid>
{
    public override void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        base.Configure(builder);

        builder.Property(uc => uc.ClaimType).IsRequired().HasMaxLength(100);
        builder.Property(uc => uc.ClaimValue).IsRequired().HasMaxLength(500);
    }
}
