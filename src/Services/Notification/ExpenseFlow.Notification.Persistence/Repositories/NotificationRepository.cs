using System;
using ExpenseFlow.Notification.Domain.Entities;
using ExpenseFlow.Notification.Domain.Interfaces;

namespace ExpenseFlow.Notification.Persistence.Repositories;

public class NotificationRepository : Repository<Domain.Entities.Notification, Guid>, INotificationRepository
{
    public NotificationRepository(NotificationDbContext dbContext) : base(dbContext)
    {
    }
}
