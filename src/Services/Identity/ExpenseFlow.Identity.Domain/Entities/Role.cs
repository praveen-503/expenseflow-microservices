using System;
using System.Collections.Generic;

namespace ExpenseFlow.Identity.Domain.Entities;

public class Role : BaseEntity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
