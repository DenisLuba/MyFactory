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
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken) 
            ?? throw new NotFoundException($"Product {request.ProductId} not found");

        // ---------- BOM ----------
        var bom = new List<ProductBomItemDto>();

        var units = await _db.Units
            .AsNoTracking()
            .ToDictionaryAsync(u => u.Id, u => u.Code, cancellationToken);

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
                Unit = units.TryGetValue(pm.Material.UnitId, out var unitName) ? unitName : null,
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
            .Where(w => w.Type == WarehouseType.FinishedGoods)
            .ToListAsync(cancellationToken);

        foreach (var warehouse in warehouses)
        {
            var totalQty = await _db.FinishedGoodsStocks
                .AsNoTracking()
                .Where(fgs =>
                    fgs.WarehouseId == warehouse.Id &&
                    fgs.ProductId == request.ProductId)
                .SumAsync(fgs => fgs.Qty, cancellationToken: cancellationToken);

            availability.Add(new ProductAvailabilityDto
            {
                WarehouseId = warehouse.Id,
                WarehouseName = warehouse.Name,
                AvailableQty = totalQty
            });
        }

        return new ProductDetailsDto
        {
            Id = product.Id,
            Sku = product.Sku,
            Name = product.Name,
            ProductTypeId = product.ProductTypeId,
            PlanPerHour = product.PlanPerHour,
            Description = product.Description,
            Version = product.Version,
            Status = product.Status,

            MaterialsCost = materialsCost,
            ProductionCost = productionCost,

            Bom = bom,
            ProductionCosts = productionCosts,
            Availability = availability
        };
    }
}

