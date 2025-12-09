using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFactory.Domain.Entities.Materials;
using MyFactory.Infrastructure.Persistence;

namespace MyFactory.Infrastructure.Repositories;

public class MaterialRepository : EfRepository<Material>, IMaterialRepository
{
    private readonly ApplicationDbContext _dbContext;

    public MaterialRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Material?> GetByIdWithHistoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Materials
            .Include(material => material.MaterialType)
            .Include(material => material.PriceHistory)
                .ThenInclude(history => history.Supplier)
            .FirstOrDefaultAsync(material => material.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Material>> GetByTypeAsync(Guid? materialTypeId, CancellationToken cancellationToken = default)
    {
        IQueryable<Material> query = _dbContext.Materials
            .Include(material => material.MaterialType);

        if (materialTypeId.HasValue)
        {
            query = query.Where(material => material.MaterialTypeId == materialTypeId.Value);
        }

        return await query.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task AddPriceHistoryAsync(MaterialPriceHistory entry, CancellationToken cancellationToken = default)
    {
        await _dbContext.MaterialPriceHistoryEntries.AddAsync(entry, cancellationToken);
    }
}
