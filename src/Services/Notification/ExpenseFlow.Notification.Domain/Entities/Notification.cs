using System;
using ExpenseFlow.Notification.Domain.Enums;

namespace ExpenseFlow.Notification.Domain.Entities;

public class Notification : AuditableEntity<Guid>
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public NotificationStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime? SentAt { get; set; }
}
