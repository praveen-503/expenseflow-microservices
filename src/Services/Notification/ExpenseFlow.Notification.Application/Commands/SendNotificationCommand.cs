using System;
using MediatR;
using ExpenseFlow.Notification.Domain.Common;
using ExpenseFlow.Notification.Domain.Enums;

namespace ExpenseFlow.Notification.Application.Commands;

public record SendNotificationCommand(
    Guid UserId,
    string Title,
    string Message,
    NotificationType Type) : IRequest<Result<Guid>>;
