using Microsoft.EntityFrameworkCore;
using MyFactory.Infrastructure.Persistence;

namespace MyFactory.Infrastructure.Repositories;

public class BaseRepository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext DbContext;

    public BaseRepository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public IQueryable<T> AsQueryable(bool asNoTracking = false)
    {
        var query = DbContext.Set<T>().AsQueryable();
        return asNoTracking ? query.AsNoTracking() : query;
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbContext.Set<T>().FindAsync([id], cancellationToken);
    }

    public async Task<IReadOnlyList<T>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await AsQueryable(asNoTracking: true).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await DbContext.Set<T>().AddAsync(entity, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbContext.Set<T>().Update(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(T entity, CancellationToken cancellationToken = default)
    {
        DbContext.Set<T>().Remove(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}
