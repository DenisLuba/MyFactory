using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Materials;

namespace MyFactory.Application.Features.Materials.GetMaterialDetails;

public sealed class GetMaterialDetailsQueryHandler : IRequestHandler<GetMaterialDetailsQuery, MaterialDetailsDto>
{
    private readonly IApplicationDbContext _db;

    public GetMaterialDetailsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<MaterialDetailsDto> Handle(GetMaterialDetailsQuery request, CancellationToken cancellationToken)
    {
        // 1. Основная информация о материале
        var material = await _db.Materials
            .AsNoTracking()
            .Include(m => m.MaterialType)
            .Include(m => m.Unit)
            .FirstOrDefaultAsync(m => m.Id == request.MaterialId, cancellationToken);

        if (material is null)
            throw new NotFoundException($"Material with Id {request.MaterialId} not found");

        if (material.Unit is null)
            throw new NotFoundException($"Unit for Material with Id {request.MaterialId} not found");

        if (material.MaterialType is null)
            throw new NotFoundException($"MaterialType for Material with Id {request.MaterialId} not found");
        
        // 2. Остатки по складам
        var warehouseStocks = await _db.WarehouseMaterials
            .AsNoTracking()
            .Where(wm => wm.MaterialId == request.MaterialId)
            .Join(_db.Warehouses.AsNoTracking(), wm => wm.WarehouseId, w => w.Id, (wm, w) => new { wm, w })
            .GroupBy(x => new { x.w.Id, x.w.Name })
            .Select(g => new WarehouseQtyDto
            {
                WarehouseId = g.Key.Id,
                WarehouseName = g.Key.Name,
                Qty = g.Sum(x => x.wm.Qty),
                UnitCode = material.Unit.Code
            })
            .ToListAsync(cancellationToken);

        // 3. История закупок
        var purchaseHistory = await _db.MaterialPurchaseOrderItems
            .AsNoTracking()
            .Where(item => item.MaterialId == request.MaterialId)
            .Join(_db.MaterialPurchaseOrders.AsNoTracking(), item => item.PurchaseOrderId, order => order.Id, (item, order) => new { item, order })
            .Join(_db.Suppliers.AsNoTracking(), io => io.order.SupplierId, s => s.Id, (io, s) => new { io.item, io.order, supplier = s })
            .OrderByDescending(x => x.order.OrderDate)
            .Select(x => new MaterialPurchaseHistoryDto
            {
                SupplierId = x.supplier.Id,
                SupplierName = x.supplier.Name,
                Qty = x.item.Qty,
                UnitPrice = x.item.UnitPrice,
                PurchaseDate = x.order.OrderDate
            })
            .ToListAsync(cancellationToken);

        var totalQty = warehouseStocks.Sum(x => x.Qty);

        return new MaterialDetailsDto
        {
            Id = material.Id,
            Name = material.Name,
            MaterialType = material.MaterialType.Name,
            UnitCode = material.Unit.Code,
            Color = material.Color,
            TotalQty = totalQty,
            Warehouses = warehouseStocks,
            PurchaseHistory = purchaseHistory
        };
    }
}
