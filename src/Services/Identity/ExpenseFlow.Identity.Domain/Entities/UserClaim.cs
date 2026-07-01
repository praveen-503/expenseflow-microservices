using System;

namespace ExpenseFlow.Identity.Domain.Entities;

public class UserClaim : BaseEntity<Guid>
{
    public Guid UserId { get; set; }
    public string ClaimType { get; set; } = string.Empty;
    public string ClaimValue { get; set; } = string.Empty;
}
