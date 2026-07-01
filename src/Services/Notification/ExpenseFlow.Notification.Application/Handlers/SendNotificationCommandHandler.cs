using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ExpenseFlow.Notification.Domain.Common;
using ExpenseFlow.Notification.Domain.Entities;
using ExpenseFlow.Notification.Domain.Interfaces;
using ExpenseFlow.Notification.Domain.Enums;
using ExpenseFlow.Notification.Application.Commands;

namespace ExpenseFlow.Notification.Application.Handlers;

public class SendNotificationCommandHandler : IRequestHandler<SendNotificationCommand, Result<Guid>>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SendNotificationCommandHandler(
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork)
    {
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = new Domain.Entities.Notification
        {
            UserId = request.UserId,
            Title = request.Title,
            Message = request.Message,
            Type = request.Type,
            Status = NotificationStatus.Pending
        };

        await _notificationRepository.AddAsync(notification, cancellationToken);
        // Unit of work is saved automatically for Commands in TransactionBehavior

        return Result<Guid>.Success(notification.Id);
    }
}
