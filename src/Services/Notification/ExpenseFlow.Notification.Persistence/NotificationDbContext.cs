using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using ExpenseFlow.Notification.Domain.Entities;
using ExpenseFlow.Notification.Domain.Interfaces;
using ExpenseFlow.Notification.Application.Interfaces;

namespace ExpenseFlow.Notification.Persistence;

public class NotificationDbContext : DbContext, IApplicationDbContext, IUnitOfWork
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Entities.Notification> Notifications => Set<Domain.Entities.Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotificationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}
