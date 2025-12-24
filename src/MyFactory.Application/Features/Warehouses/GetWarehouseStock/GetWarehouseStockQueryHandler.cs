using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.DTOs.Warehouses;
using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.Application.Features.Warehouses.GetWarehouseStock;

public sealed class GetWarehouseStockQueryHandler
    : IRequestHandler<GetWarehouseStockQuery, IReadOnlyList<WarehouseStockItemDto>>
{
    private readonly IApplicationDbContext _db;

    public GetWarehouseStockQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<WarehouseStockItemDto>> Handle(
        GetWarehouseStockQuery request,
        CancellationToken cancellationToken)
    {
        var warehouse = await _db.Warehouses
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == request.WarehouseId,
                cancellationToken);

        if (warehouse is null)
            throw new NotFoundException("Warehouse not found");

        return warehouse.Type switch
        {
            WarehouseType.Materials or WarehouseType.Aux
                => await GetMaterialStockAsync(request.WarehouseId, cancellationToken),

            WarehouseType.FinishedGoods
                => await GetFinishedGoodsStockAsync(request.WarehouseId, cancellationToken),

            _ => Array.Empty<WarehouseStockItemDto>()
        };
    }

    private async Task<IReadOnlyList<WarehouseStockItemDto>> GetMaterialStockAsync(
        Guid warehouseId,
        CancellationToken cancellationToken)
    {
        return await _db.WarehouseMaterials
            .AsNoTracking()
            .Where(x => x.WarehouseId == warehouseId)
            .Include(x => x.Material)
                .ThenInclude(m => m!.Unit)
            .OrderBy(x => x.Material!.Name)
            .Select(x => new WarehouseStockItemDto
            {
                ItemId = x.MaterialId,
                Name = x.Material!.Name,
                Qty = x.Qty,
                UnitCode = x.Material!.Unit!.Code
            })
            .ToListAsync(cancellationToken);
    }

    private async Task<IReadOnlyList<WarehouseStockItemDto>> GetFinishedGoodsStockAsync(
        Guid warehouseId,
        CancellationToken cancellationToken)
    {
        return await _db.FinishedGoodsStocks
            .AsNoTracking()
            .Where(x => x.WarehouseId == warehouseId)
            .Include(x => x.Product)
            .OrderBy(x => x.Product!.Name)
            .Select(x => new WarehouseStockItemDto
            {
                ItemId = x.ProductId,
                Name = x.Product!.Name,
                Qty = x.Qty,
                UnitCode = null
            })
            .ToListAsync(cancellationToken);
    }
}
