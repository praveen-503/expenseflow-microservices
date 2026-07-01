using System;
using ExpenseFlow.Notification.Domain.Entities;
using ExpenseFlow.Notification.Domain.Interfaces;

namespace ExpenseFlow.Notification.Domain.Interfaces;

public interface INotificationRepository : IRepository<Entities.Notification, Guid>
{
}
