using System;
using ExpenseFlow.Notification.Application.Interfaces;

namespace ExpenseFlow.Notification.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
