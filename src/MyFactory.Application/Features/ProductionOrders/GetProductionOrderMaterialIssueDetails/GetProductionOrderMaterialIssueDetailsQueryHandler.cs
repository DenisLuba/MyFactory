using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.ProductionOrders;
using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionOrderMaterialIssueDetails;

public sealed class GetProductionOrderMaterialIssueDetailsQueryHandler
    : IRequestHandler<
        GetProductionOrderMaterialIssueDetailsQuery,
        ProductionOrderMaterialIssueDetailsDto>
{
    private readonly IApplicationDbContext _db;

    public GetProductionOrderMaterialIssueDetailsQueryHandler(
        IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<ProductionOrderMaterialIssueDetailsDto> Handle(
        GetProductionOrderMaterialIssueDetailsQuery request,
        CancellationToken cancellationToken)
    {
        // 1. Production order
        var po = await _db.ProductionOrders
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == request.ProductionOrderId,
                cancellationToken)
            ?? throw new NotFoundException("Production order not found");

        // 2. SalesOrderItem -> Product
        var soi = await _db.SalesOrderItems
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == po.SalesOrderItemId,
                cancellationToken)
            ?? throw new NotFoundException("Sales order item not found");

        // 3. Материал должен входить в спецификацию товара
        var materialSpec = await _db.ProductMaterials
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.ProductId == soi.ProductId
                  && x.MaterialId == request.MaterialId,
                cancellationToken);

        if (materialSpec is null)
            throw new NotFoundException("Material not found in product specification");

        // 4. Основной запрос (материал + склады)
        var query =
            from pm in _db.ProductMaterials.AsNoTracking()
            join m in _db.Materials.AsNoTracking()
                on pm.MaterialId equals m.Id
            join wm in _db.WarehouseMaterials.AsNoTracking()
                on pm.MaterialId equals wm.MaterialId
            join w in _db.Warehouses.AsNoTracking()
                on wm.WarehouseId equals w.Id
            where pm.ProductId == soi.ProductId
               && pm.MaterialId == request.MaterialId
               && w.Type == WarehouseType.Materials
            group new { pm, m, wm, w }
            by new { m.Id, m.Name, pm.QtyPerUnit } into g
            select new
            {
                g.Key.Id,
                g.Key.Name,
                RequiredQty = g.Key.QtyPerUnit * po.QtyPlanned,
                AvailableQty = g.Sum(x => x.wm.Qty),
                Warehouses = g
                    .OrderByDescending(x => x.wm.Qty)
                    .Select(x => new ProductionOrderMaterialWarehouseDto
                    {
                        WarehouseId = x.w.Id,
                        WarehouseName = x.w.Name,
                        AvailableQty = x.wm.Qty
                    })
                    .ToList()
            };

        var data = await query.FirstAsync(cancellationToken);

        // 5. Формирование результата
        return new ProductionOrderMaterialIssueDetailsDto
        {
            Material = new ProductionOrderMaterialDto
            {
                MaterialId = data.Id,
                MaterialName = data.Name,
                RequiredQty = data.RequiredQty,
                AvailableQty = data.AvailableQty,
                MissingQty = Math.Max(0, data.RequiredQty - data.AvailableQty)
            },
            Warehouses = data.Warehouses
        };
    }
}