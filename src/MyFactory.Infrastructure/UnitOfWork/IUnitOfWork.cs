using System.Threading;
using System.Threading.Tasks;

namespace MyFactory.Infrastructure.Persistence.UnitOfWork;

public interface IUnitOfWork
{
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task CommitAsync(CancellationToken cancellationToken = default);

    Task RollbackAsync(CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
