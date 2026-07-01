using System.Threading;
using System.Threading.Tasks;

namespace ExpenseFlow.Identity.Application.Interfaces;

public interface IApplicationDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
