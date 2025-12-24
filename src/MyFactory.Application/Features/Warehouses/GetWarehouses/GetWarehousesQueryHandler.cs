using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Warehouses;

namespace MyFactory.Application.Features.Warehouses.GetWarehouses;

public sealed class GetWarehousesQueryHandler
    : IRequestHandler<GetWarehousesQuery, IReadOnlyList<WarehouseListItemDto>>
{
    private readonly IApplicationDbContext _db;

    public GetWarehousesQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<WarehouseListItemDto>> Handle(
        GetWarehousesQuery request,
        CancellationToken cancellationToken)
    {
        var query = _db.Warehouses.AsNoTracking();

        if (!request.IncludeInactive)
            query = query.Where(w => w.IsActive);

        return await query
            .OrderBy(w => w.Name)
            .Select(w => new WarehouseListItemDto
            {
                Id = w.Id,
                Name = w.Name,
                Type = w.Type,
                IsActive = w.IsActive
            })
            .ToListAsync(cancellationToken);
    }
}