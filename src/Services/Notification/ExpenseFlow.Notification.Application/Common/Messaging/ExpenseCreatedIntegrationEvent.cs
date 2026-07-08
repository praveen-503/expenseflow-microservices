using System;
using ExpenseFlow.Notification.Application.Common.Messaging;

namespace ExpenseFlow.Notification.Application.Common.Messaging;

public class ExpenseCreatedIntegrationEvent : BaseIntegrationEvent
{
    public Guid ExpenseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime ExpenseDate { get; set; }
    public Guid CategoryId { get; set; }
    public Guid UserId { get; set; }
}
