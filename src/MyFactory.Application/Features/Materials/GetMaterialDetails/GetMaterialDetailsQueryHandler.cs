using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Materials;
using System.Diagnostics;

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
        try
        {
            // 1. получаем сущность материала по Id, включаем таблицы кода и типа
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

            // 2. наличие на складах
            var warehouseStocks = await _db.WarehouseMaterials
                .AsNoTracking()
                .Where(wm => wm.MaterialId == request.MaterialId)
                .Join
                (
                    inner: _db.Warehouses.AsNoTracking(), 
                    outerKeySelector: wm => wm.WarehouseId, 
                    innerKeySelector: w => w.Id, 
                    resultSelector: (wm, w) => new { wm, w }
                )
                .GroupBy(x => new { x.w.Id, x.w.Name })
                .Select(g => new WarehouseQtyDto
                {
                    WarehouseId = g.Key.Id,
                    WarehouseName = g.Key.Name,
                    Qty = g.Sum(x => x.wm.Qty),
                    UnitCode = material.Unit.Code
                })
                .Where(w => w.Qty > 0)
                .ToListAsync(cancellationToken);

            // 3. история покупок
            var purchaseHistory = await _db.MaterialPurchaseOrderItems
                .AsNoTracking()
                .Where(item => item.MaterialId == request.MaterialId)
                .Join
                (
                    inner: _db.MaterialPurchaseOrders.AsNoTracking(), 
                    outerKeySelector: item => item.PurchaseOrderId, 
                    innerKeySelector: order => order.Id, 
                    resultSelector: (item, order) => new { item, order }
                )
                .Join
                (
                    inner: _db.Suppliers.AsNoTracking(), 
                    outerKeySelector: io => io.order.SupplierId, 
                    innerKeySelector: s => s.Id, 
                    resultSelector: (io, s) => new { io.item, io.order, supplier = s }
                )
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
                Description = material.Description,
                TotalQty = totalQty,
                Warehouses = warehouseStocks,
                PurchaseHistory = purchaseHistory
            };
        }
        catch (Exception ex) when (ex is NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Trace.TraceError("Error while getting material details for Id {0}: {1}", request.MaterialId, ex);
            throw new Exception("An error occurred while retrieving material details. Please try again later.");
        }
    }
}