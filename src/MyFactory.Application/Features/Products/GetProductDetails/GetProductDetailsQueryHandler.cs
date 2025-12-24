using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Products;
using MyFactory.Domain.Entities.Inventory;

namespace MyFactory.Application.Features.Products.GetProductDetails;

public sealed class GetProductDetailsQueryHandler
    : IRequestHandler<GetProductDetailsQuery, ProductDetailsDto>
{
    private readonly IApplicationDbContext _db;

    public GetProductDetailsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<ProductDetailsDto> Handle(
        GetProductDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var product = await _db.Products
            .AsNoTracking()
            .Include(p => p.ProductMaterials)
                .ThenInclude(pm => pm.Material)
            .Include(p => p.ProductDepartmentCosts)
                .ThenInclude(dc => dc.Department)
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

        if (product is null)
            throw new NotFoundException($"Product {request.ProductId} not found");

        // ---------- BOM ----------
        var bom = new List<ProductBomItemDto>();

        foreach (var pm in product.ProductMaterials)
        {
            var lastPrice = await _db.MaterialPurchaseOrderItems
                .AsNoTracking()
                .Where(i => i.MaterialId == pm.MaterialId)
                .OrderByDescending(i => i.CreatedAt)
                .Select(i => i.UnitPrice)
                .FirstOrDefaultAsync(cancellationToken);

            bom.Add(new ProductBomItemDto
            {
                MaterialId = pm.MaterialId,
                MaterialName = pm.Material!.Name,
                QtyPerUnit = pm.QtyPerUnit,
                LastUnitPrice = lastPrice
            });
        }

        var materialsCost = bom.Sum(x => x.TotalCost);

        // ---------- Производственные затраты ----------
        var productionCosts = product.ProductDepartmentCosts
            .Where(x => x.IsActive)
            .Select(x => new ProductDepartmentCostDto
            {
                DepartmentId = x.DepartmentId,
                DepartmentName = x.Department!.Name,
                CutCost = x.CutCostPerUnit,
                SewingCost = x.SewingCostPerUnit,
                PackCost = x.PackCostPerUnit,
                Expenses = x.ExpensesPerUnit
            })
            .ToList();

        var productionCost = productionCosts.Sum(x => x.Total);

        // ---------- Доступность ----------
        var availability = new List<ProductAvailabilityDto>();

        var warehouses = await _db.Warehouses
            .AsNoTracking()
            .Where(w => w.Type == WarehouseType.Materials)
            .ToListAsync(cancellationToken);

        foreach (var warehouse in warehouses)
        {
            int minPossible = int.MaxValue;

            foreach (var pm in product.ProductMaterials)
            {
                var stockQty = await _db.WarehouseMaterials
                    .AsNoTracking()
                    .Where(wm =>
                        wm.WarehouseId == warehouse.Id &&
                        wm.MaterialId == pm.MaterialId)
                    .Select(wm => wm.Qty)
                    .FirstOrDefaultAsync(cancellationToken);

                var possible = pm.QtyPerUnit > 0
                    ? (int)(stockQty / pm.QtyPerUnit)
                    : 0;

                minPossible = Math.Min(minPossible, possible);
            }

            availability.Add(new ProductAvailabilityDto
            {
                WarehouseId = warehouse.Id,
                WarehouseName = warehouse.Name,
                AvailableQty = minPossible == int.MaxValue ? 0 : minPossible
            });
        }

        return new ProductDetailsDto
        {
            Id = product.Id,
            Name = product.Name,
            PlanPerHour = product.PlanPerHour,

            MaterialsCost = materialsCost,
            ProductionCost = productionCost,

            Bom = bom,
            ProductionCosts = productionCosts,
            Availability = availability
        };
    }
}

