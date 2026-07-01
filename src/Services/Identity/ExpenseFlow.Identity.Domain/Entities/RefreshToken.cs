using System;

namespace ExpenseFlow.Identity.Domain.Entities;

public class RefreshToken : BaseEntity<Guid>
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRevoked { get; set; }
}
