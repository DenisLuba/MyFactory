using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFactory.Domain.Common;
using MyFactory.Infrastructure.Persistence;
using MyFactory.Infrastructure.Repositories.Specifications;

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

    public IQueryable<T> AsQueryable(bool asNoTracking = false, bool includeSoftDeleted = false)
    {
        IQueryable<T> query = includeSoftDeleted ? _set.IgnoreQueryFilters() : _set.AsQueryable();
        return asNoTracking ? query.AsNoTracking() : query;
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

    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).ToListAsync(cancellationToken);
    }

    public Task<T?> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    public Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return ApplySpecification(specification, ignorePaging: true).CountAsync(cancellationToken);
    }

    public async Task<PaginatedResult<T>> PaginatedListAsync(ISpecification<T> specification, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = ApplySpecification(specification, ignorePaging: true);
        var totalCount = await query.CountAsync(cancellationToken);

        var skip = (pageNumber - 1) * pageSize;
        var items = await query.Skip(skip).Take(pageSize).ToListAsync(cancellationToken);

        return PaginatedResult<T>.Create(items, totalCount, pageNumber, pageSize);
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
        entity.IsDeleted = true;
        DbContext.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> specification, bool ignorePaging = false)
    {
        return SpecificationEvaluator<T>.GetQuery(_set.AsQueryable(), specification, ignorePaging);
    }
}
