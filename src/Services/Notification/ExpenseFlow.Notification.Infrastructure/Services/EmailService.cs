using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using ExpenseFlow.Notification.Application.Interfaces;

namespace ExpenseFlow.Notification.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        // Mock email sending by printing to structured logs
        _logger.LogInformation("Sending email notification to recipient {Recipient}. Subject: '{Subject}', Body: '{Body}'", to, subject, body);
        
        return Task.CompletedTask;
    }
}
