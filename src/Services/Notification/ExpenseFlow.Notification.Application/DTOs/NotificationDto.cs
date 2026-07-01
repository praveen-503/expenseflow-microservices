using System;
using ExpenseFlow.Notification.Domain.Enums;

namespace ExpenseFlow.Notification.Application.DTOs;

public record NotificationDto(
    Guid Id,
    Guid UserId,
    string Title,
    string Message,
    NotificationType Type,
    NotificationStatus Status,
    string? ErrorMessage,
    DateTime? SentAt,
    DateTime CreatedAt);
