using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Inventory;

namespace MyFactory.Application.Features.Inventory.Queries.GetMaterialInventory;

public sealed class GetMaterialInventoryQueryHandler : IRequestHandler<GetMaterialInventoryQuery, InventoryMaterialSummaryDto>
{
    private readonly IApplicationDbContext _context;

    public GetMaterialInventoryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryMaterialSummaryDto> Handle(GetMaterialInventoryQuery request, CancellationToken cancellationToken)
    {
        var material = await _context.Materials
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == request.MaterialId, cancellationToken)
            ?? throw new InvalidOperationException("Material not found.");

        var inventoryRows = await (
            from item in _context.InventoryItems.AsNoTracking()
            where item.MaterialId == request.MaterialId
            join warehouse in _context.Warehouses.AsNoTracking() on item.WarehouseId equals warehouse.Id
            select new
            {
                item.Quantity,
                item.ReservedQuantity,
                item.AveragePrice,
                warehouse.Id,
                warehouse.Name,
                warehouse.Type,
                warehouse.Location
            }).ToListAsync(cancellationToken);

        if (inventoryRows.Count == 0)
        {
            return new InventoryMaterialSummaryDto(
                material.Id,
                material.Name,
                material.Unit,
                0,
                0,
                0,
                0,
                0,
                Array.Empty<InventoryWarehouseBreakdownDto>());
        }

        var totalQuantity = inventoryRows.Sum(row => row.Quantity);
        var reservedQuantity = inventoryRows.Sum(row => row.ReservedQuantity);
        var availableQuantity = totalQuantity - reservedQuantity;
        var totalValue = inventoryRows.Sum(row => row.Quantity * row.AveragePrice);
        var averagePrice = totalQuantity > 0 ? totalValue / totalQuantity : 0m;

        var warehouses = inventoryRows
            .Select(row => new InventoryWarehouseBreakdownDto(
                row.Id,
                row.Name,
                row.Type,
                row.Location,
                row.Quantity,
                row.ReservedQuantity,
                row.Quantity - row.ReservedQuantity,
                row.AveragePrice,
                row.Quantity * row.AveragePrice))
            .OrderBy(dto => dto.WarehouseName)
            .ToArray();

        return new InventoryMaterialSummaryDto(
            material.Id,
            material.Name,
            material.Unit,
            totalQuantity,
            reservedQuantity,
            availableQuantity,
            averagePrice,
            totalValue,
            warehouses);
    }
}
