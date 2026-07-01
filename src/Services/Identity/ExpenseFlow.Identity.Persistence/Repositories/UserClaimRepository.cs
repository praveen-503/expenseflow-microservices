using System;
using ExpenseFlow.Identity.Domain.Entities;
using ExpenseFlow.Identity.Domain.Interfaces;

namespace ExpenseFlow.Identity.Persistence.Repositories;

public class UserClaimRepository : Repository<UserClaim, Guid>, IUserClaimRepository
{
    public UserClaimRepository(IdentityDbContext dbContext) : base(dbContext)
    {
    }
}
