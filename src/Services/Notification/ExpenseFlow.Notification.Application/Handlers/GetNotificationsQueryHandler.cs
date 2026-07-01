using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ExpenseFlow.Notification.Domain.Common;
using ExpenseFlow.Notification.Domain.Interfaces;
using ExpenseFlow.Notification.Application.DTOs;
using ExpenseFlow.Notification.Application.Queries;

namespace ExpenseFlow.Notification.Application.Handlers;

public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, Result<IEnumerable<NotificationDto>>>
{
    private readonly INotificationRepository _notificationRepository;

    public GetNotificationsQueryHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<Result<IEnumerable<NotificationDto>>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        // For learning architecture, list all matching items via list all or a custom specification in the future
        var notifications = await _notificationRepository.ListAllAsync(cancellationToken);

        var dtos = notifications
            .Where(n => n.UserId == request.UserId)
            .Select(n => new NotificationDto(
                n.Id,
                n.UserId,
                n.Title,
                n.Message,
                n.Type,
                n.Status,
                n.ErrorMessage,
                n.SentAt,
                n.CreatedAt
            ));

        return Result<IEnumerable<NotificationDto>>.Success(dtos);
    }
}
