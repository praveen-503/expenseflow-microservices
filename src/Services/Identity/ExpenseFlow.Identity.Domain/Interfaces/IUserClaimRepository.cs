using System;
using ExpenseFlow.Identity.Domain.Entities;
using ExpenseFlow.Identity.Domain.Interfaces;

namespace ExpenseFlow.Identity.Domain.Interfaces;

public interface IUserClaimRepository : IRepository<UserClaim, Guid>
{
}
