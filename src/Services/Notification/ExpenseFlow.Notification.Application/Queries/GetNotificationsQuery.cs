using System;
using System.Collections.Generic;
using MediatR;
using ExpenseFlow.Notification.Domain.Common;
using ExpenseFlow.Notification.Application.DTOs;

namespace ExpenseFlow.Notification.Application.Queries;

public record GetNotificationsQuery(Guid UserId) : IRequest<Result<IEnumerable<NotificationDto>>>;
