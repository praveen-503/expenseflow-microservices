using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ExpenseFlow.Notification.Domain.Common;
using ExpenseFlow.Notification.Domain.Entities;
using ExpenseFlow.Notification.Domain.Interfaces;
using ExpenseFlow.Notification.Domain.Enums;
using ExpenseFlow.Notification.Application.Commands;
using ExpenseFlow.Notification.Application.Interfaces;

namespace ExpenseFlow.Notification.Application.Handlers;

public class SendNotificationCommandHandler : IRequestHandler<SendNotificationCommand, Result<Guid>>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly ILogger<SendNotificationCommandHandler> _logger;

    public SendNotificationCommandHandler(
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork,
        IEmailService emailService,
        ILogger<SendNotificationCommandHandler> logger)
    {
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _logger = logger;
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
        
        // Dispatch simulated email
        if (request.Type == NotificationType.Email)
        {
            try
            {
                await _emailService.SendEmailAsync(request.UserId.ToString(), request.Title, request.Message, cancellationToken);
                notification.Status = NotificationStatus.Sent;
                notification.SentAt = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                notification.Status = NotificationStatus.Failed;
                notification.ErrorMessage = ex.Message;
                _logger.LogError(ex, "Failed to send email notification to user {UserId}. Notification will be retried.", request.UserId);
                throw;
            }
        }

        // Unit of work is saved automatically for Commands in TransactionBehavior
        return Result<Guid>.Success(notification.Id);
    }
}
