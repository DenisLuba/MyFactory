using System.Threading;
using System.Threading.Tasks;
using MyFactory.Domain.Common;
using MyFactory.Infrastructure.Repositories;

namespace MyFactory.Infrastructure.Persistence.UnitOfWork;

public interface IUnitOfWork
{
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;

    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task CommitAsync(CancellationToken cancellationToken = default);

    Task RollbackAsync(CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
