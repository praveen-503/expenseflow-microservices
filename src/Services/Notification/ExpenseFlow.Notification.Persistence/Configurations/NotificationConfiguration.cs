using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ExpenseFlow.Notification.Domain.Entities;

namespace ExpenseFlow.Notification.Persistence.Configurations;

public class NotificationConfiguration : BaseEntityConfiguration<Domain.Entities.Notification, Guid>
{
    public override void Configure(EntityTypeBuilder<Domain.Entities.Notification> builder)
    {
        base.Configure(builder);

        builder.Property(n => n.UserId).IsRequired();
        builder.Property(n => n.Title).IsRequired().HasMaxLength(200);
        builder.Property(n => n.Message).IsRequired().HasMaxLength(2000);
        builder.Property(n => n.Type).IsRequired();
        builder.Property(n => n.Status).IsRequired();
        builder.Property(n => n.ErrorMessage).HasMaxLength(1000);
    }
}
