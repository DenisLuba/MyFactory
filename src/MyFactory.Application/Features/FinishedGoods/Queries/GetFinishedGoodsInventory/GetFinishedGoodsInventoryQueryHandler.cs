using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.FinishedGoods;
using MyFactory.Application.Features.FinishedGoods.Common;

namespace MyFactory.Application.Features.FinishedGoods.Queries.GetFinishedGoodsInventory;

public sealed class GetFinishedGoodsInventoryQueryHandler : IRequestHandler<GetFinishedGoodsInventoryQuery, IReadOnlyCollection<FinishedGoodsInventoryDto>>
{
    private readonly IApplicationDbContext _context;

    public GetFinishedGoodsInventoryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<FinishedGoodsInventoryDto>> Handle(GetFinishedGoodsInventoryQuery request, CancellationToken cancellationToken)
    {
        var query = _context.FinishedGoodsInventories
            .AsNoTracking()
            .AsQueryable();

        if (request.SpecificationId.HasValue)
        {
            query = query.Where(entity => entity.SpecificationId == request.SpecificationId.Value);
        }

        if (request.WarehouseId.HasValue)
        {
            query = query.Where(entity => entity.WarehouseId == request.WarehouseId.Value);
        }

        var inventories = await query
            .OrderByDescending(entity => entity.UpdatedAt)
            .ThenBy(entity => entity.Id)
            .ToListAsync(cancellationToken);

        return await FinishedGoodsInventoryDtoFactory.CreateAsync(_context, inventories, cancellationToken);
    }
}
