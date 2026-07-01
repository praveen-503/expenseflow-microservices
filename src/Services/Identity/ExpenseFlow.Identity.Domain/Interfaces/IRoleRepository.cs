using System;
using System.Threading;
using System.Threading.Tasks;
using ExpenseFlow.Identity.Domain.Entities;
using ExpenseFlow.Identity.Domain.Interfaces;

namespace ExpenseFlow.Identity.Domain.Interfaces;

public interface IRoleRepository : IRepository<Role, Guid>
{
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
