using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExpenseFlow.Identity.Domain.Entities;

namespace ExpenseFlow.Identity.Persistence.Configurations;

public class RefreshTokenConfiguration : BaseEntityConfiguration<RefreshToken, Guid>
{
    public override void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        base.Configure(builder);

        builder.Property(rt => rt.Token).IsRequired().HasMaxLength(256);
    }
}
