using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ExpenseFlow.Notification.Domain.Common;
using ExpenseFlow.Notification.Domain.Enums;
using ExpenseFlow.Notification.Domain.Interfaces;
using ExpenseFlow.Notification.Application.Commands;

namespace ExpenseFlow.Notification.Application.Handlers;

public class UpdateNotificationStatusCommandHandler : IRequestHandler<UpdateNotificationStatusCommand, Result>
{
    private readonly INotificationRepository _notificationRepository;

    public UpdateNotificationStatusCommandHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<Result> Handle(UpdateNotificationStatusCommand request, CancellationToken cancellationToken)
    {
        var notification = await _notificationRepository.GetByIdAsync(request.Id, cancellationToken);
        if (notification == null)
        {
            return Result.Failure(new Error("Notification.NotFound", "Notification not found."));
        }

        notification.Status = request.Status;
        notification.ErrorMessage = request.ErrorMessage;
        if (request.Status == NotificationStatus.Sent)
        {
            notification.SentAt = DateTime.UtcNow;
        }

        _notificationRepository.Update(notification);

        return Result.Success();
    }
}
