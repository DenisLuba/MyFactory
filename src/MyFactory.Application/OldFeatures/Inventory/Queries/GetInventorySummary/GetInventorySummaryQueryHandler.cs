using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Inventory;

namespace MyFactory.Application.OldFeatures.Inventory.Queries.GetInventorySummary;

public sealed class GetInventorySummaryQueryHandler : IRequestHandler<GetInventorySummaryQuery, IReadOnlyCollection<InventoryMaterialSummaryDto>>
{
    private readonly IApplicationDbContext _context;

    public GetInventorySummaryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<InventoryMaterialSummaryDto>> Handle(GetInventorySummaryQuery request, CancellationToken cancellationToken)
    {
        var inventoryRows = await (
            from item in _context.InventoryItems.AsNoTracking()
            join material in _context.Materials.AsNoTracking() on item.MaterialId equals material.Id
            join warehouse in _context.Warehouses.AsNoTracking() on item.WarehouseId equals warehouse.Id
            select new
            {
                item.Quantity,
                item.ReservedQuantity,
                item.AveragePrice,
                MaterialId = material.Id,
                MaterialName = material.Name,
                MaterialUnit = material.Unit,
                WarehouseId = warehouse.Id,
                WarehouseName = warehouse.Name,
                WarehouseType = warehouse.Type,
                WarehouseLocation = warehouse.Location
            }).ToListAsync(cancellationToken);

        if (inventoryRows.Count == 0)
        {
            return Array.Empty<InventoryMaterialSummaryDto>();
        }

        var summaries = inventoryRows
            .GroupBy(row => new { row.MaterialId, row.MaterialName, row.MaterialUnit })
            .Select(group =>
            {
                var totalQuantity = group.Sum(row => row.Quantity);
                var reservedQuantity = group.Sum(row => row.ReservedQuantity);
                var availableQuantity = totalQuantity - reservedQuantity;
                var totalValue = group.Sum(row => row.Quantity * row.AveragePrice);
                var averagePrice = totalQuantity > 0 ? totalValue / totalQuantity : 0m;

                var warehouses = group
                    .Select(row => new InventoryWarehouseBreakdownDto(
                        row.WarehouseId,
                        row.WarehouseName,
                        row.WarehouseType,
                        row.WarehouseLocation,
                        row.Quantity,
                        row.ReservedQuantity,
                        row.Quantity - row.ReservedQuantity,
                        row.AveragePrice,
                        row.Quantity * row.AveragePrice))
                    .OrderBy(dto => dto.WarehouseName)
                    .ToArray();

                return new InventoryMaterialSummaryDto(
                    group.Key.MaterialId,
                    group.Key.MaterialName,
                    group.Key.MaterialUnit,
                    totalQuantity,
                    reservedQuantity,
                    availableQuantity,
                    averagePrice,
                    totalValue,
                    warehouses);
            })
            .OrderBy(dto => dto.MaterialName)
            .ToArray();

        return summaries;
    }
}
