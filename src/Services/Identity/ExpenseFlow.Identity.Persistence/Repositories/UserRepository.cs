using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ExpenseFlow.Identity.Domain.Entities;
using ExpenseFlow.Identity.Domain.Interfaces;

namespace ExpenseFlow.Identity.Persistence.Repositories;

public class UserRepository : Repository<User, Guid>, IUserRepository
{
    public UserRepository(IdentityDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<User>()
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }
}
