using System.Threading;
using System.Threading.Tasks;

namespace ExpenseFlow.Expense.Application.Interfaces;

public interface IApplicationDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
