using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.Features.Warehousing.Queries.GetWarehouses;

public sealed class GetWarehousesQueryHandler : IRequestHandler<GetWarehousesQuery, IReadOnlyCollection<WarehouseDto>>
{
    private readonly IApplicationDbContext _context;

    public GetWarehousesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<WarehouseDto>> Handle(GetWarehousesQuery request, CancellationToken cancellationToken)
    {
        var warehouses = await _context.Warehouses
            .AsNoTracking()
            .OrderBy(warehouse => warehouse.Name)
            .ToListAsync(cancellationToken);

        return warehouses.Select(WarehouseDto.FromEntity).ToList();
    }
}
