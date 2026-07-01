using System;
using ExpenseFlow.Notification.Application.Common.Messaging;

namespace ExpenseFlow.Notification.Application.Common.Messaging;

public class ExpenseDeletedIntegrationEvent : BaseIntegrationEvent
{
    public Guid ExpenseId { get; set; }
    public Guid UserId { get; set; }
}
