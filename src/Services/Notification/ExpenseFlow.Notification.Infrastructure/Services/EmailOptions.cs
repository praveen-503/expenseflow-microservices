namespace ExpenseFlow.Notification.Infrastructure.Services;

public class EmailOptions
{
    public const string SectionName = "Email";

    public string SmtpServer { get; set; } = "smtp.gmail.com";
    public int Port { get; set; } = 587;
    public string SenderName { get; set; } = "ExpenseFlow";
    public string SenderEmail { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; // Gmail App Password
    public bool EnableSsl { get; set; } = true;
    public string FallbackRecipientEmail { get; set; } = string.Empty; // Used when recipient is a GUID string
}
