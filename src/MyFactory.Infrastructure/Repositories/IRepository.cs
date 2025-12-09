using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MyFactory.Domain.Common;

namespace MyFactory.Infrastructure.Repositories;

public interface IRepository<T> where T : BaseEntity
{
    IQueryable<T> AsQueryable(bool asNoTracking = false);

    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default);

    Task AddAsync(T entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

    Task RemoveAsync(T entity, CancellationToken cancellationToken = default);
}
