using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ExpenseFlow.Identity.Domain.Entities;
using ExpenseFlow.Identity.Domain.Interfaces;

namespace ExpenseFlow.Identity.Persistence.Repositories;

public class RoleRepository : Repository<Role, Guid>, IRoleRepository
{
    public RoleRepository(IdentityDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Role>()
            .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
    }
}
