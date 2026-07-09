using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using ExpenseFlow.Notification.Application.Interfaces;

namespace ExpenseFlow.Notification.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly EmailOptions _options;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailOptions> options, ILogger<EmailService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        string recipient = to;

        // If 'to' is a GUID (mock implementation where user database mapping is not yet present)
        if (Guid.TryParse(to, out _))
        {
            if (!string.IsNullOrEmpty(_options.FallbackRecipientEmail))
            {
                recipient = _options.FallbackRecipientEmail;
                _logger.LogInformation("Recipient is GUID '{Guid}'. Falling back to configured email: {FallbackEmail}", to, recipient);
            }
            else
            {
                _logger.LogWarning("Recipient is GUID '{Guid}' and no FallbackRecipientEmail is configured. Mocking the send instead.", to);
                return;
            }
        }

        try
        {
            using var mailMessage = new MailMessage
            {
                From = new MailAddress(_options.SenderEmail, _options.SenderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            
            mailMessage.To.Add(recipient);

            using var smtpClient = new SmtpClient(_options.SmtpServer, _options.Port)
            {
                Credentials = new NetworkCredential(_options.Username, _options.Password),
                EnableSsl = _options.EnableSsl
            };

            _logger.LogInformation("Sending email to {Recipient} via Gmail SMTP...", recipient);
            await smtpClient.SendMailAsync(mailMessage);
            _logger.LogInformation("Email successfully sent to {Recipient}.", recipient);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Recipient} via Gmail SMTP.", recipient);
            throw;
        }
    }
}
