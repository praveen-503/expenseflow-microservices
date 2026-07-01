using System;
using ExpenseFlow.Identity.Domain.Entities;
using ExpenseFlow.Identity.Domain.Interfaces;

namespace ExpenseFlow.Identity.Persistence.Repositories;

public class RoleRepository : Repository<Role, Guid>, IRoleRepository
{
    public RoleRepository(IdentityDbContext dbContext) : base(dbContext)
    {
    }
}
