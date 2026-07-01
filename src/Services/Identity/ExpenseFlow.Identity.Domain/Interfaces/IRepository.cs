using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExpenseFlow.Identity.Domain.Specifications;

namespace ExpenseFlow.Identity.Domain.Interfaces;

public interface IRepository<T, TId> where T : class
{
    Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    void Update(T entity);
    void Delete(T entity);
    Task<int> CountAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
}
