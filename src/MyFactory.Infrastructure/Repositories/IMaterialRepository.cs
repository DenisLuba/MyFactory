using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Infrastructure.Repositories;

public interface IMaterialRepository : IRepository<Material>
{
    Task<Material?> GetByIdWithHistoryAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Material>> GetByTypeAsync(Guid? materialTypeId, CancellationToken cancellationToken = default);

    Task AddPriceHistoryAsync(MaterialPriceHistory entry, CancellationToken cancellationToken = default);
}
