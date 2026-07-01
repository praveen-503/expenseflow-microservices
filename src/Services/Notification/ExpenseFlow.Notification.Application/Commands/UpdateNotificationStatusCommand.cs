using System;
using MediatR;
using ExpenseFlow.Notification.Domain.Common;
using ExpenseFlow.Notification.Domain.Enums;

namespace ExpenseFlow.Notification.Application.Commands;

public record UpdateNotificationStatusCommand(
    Guid Id,
    NotificationStatus Status,
    string? ErrorMessage) : IRequest<Result>;
