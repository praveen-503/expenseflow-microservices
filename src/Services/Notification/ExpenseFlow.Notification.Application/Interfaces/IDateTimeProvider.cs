using System;

namespace ExpenseFlow.Notification.Application.Interfaces;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
