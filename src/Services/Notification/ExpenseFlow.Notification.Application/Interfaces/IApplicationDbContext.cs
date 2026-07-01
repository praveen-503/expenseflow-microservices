using System.Threading;
using System.Threading.Tasks;

namespace ExpenseFlow.Notification.Application.Interfaces;

public interface IApplicationDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
