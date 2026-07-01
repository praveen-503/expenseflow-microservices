using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseFlow.Expense.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
