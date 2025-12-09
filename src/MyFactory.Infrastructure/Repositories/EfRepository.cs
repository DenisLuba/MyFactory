using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFactory.Domain.Common;
using MyFactory.Infrastructure.Persistence;

namespace MyFactory.Infrastructure.Repositories;

public class EfRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext DbContext;
    private readonly DbSet<T> _set;

    public EfRepository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
        _set = dbContext.Set<T>();
    }

    public IQueryable<T> AsQueryable(bool asNoTracking = false)
    {
        return asNoTracking ? _set.AsNoTracking() : _set.AsQueryable();
    }

    public Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _set.FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _set;
        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _set.AddAsync(entity, cancellationToken);
    }

    public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _set.Update(entity);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(T entity, CancellationToken cancellationToken = default)
    {
        _set.Remove(entity);
        return Task.CompletedTask;
    }
}
