using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.FinishedGoods;
using MyFactory.Domain.Entities.FinishedGoods;

namespace MyFactory.Application.Features.FinishedGoods.Common;

internal static class FinishedGoodsInventoryDtoFactory
{
    public static async Task<IReadOnlyCollection<FinishedGoodsInventoryDto>> CreateAsync(
        IApplicationDbContext context,
        IReadOnlyCollection<FinishedGoodsInventory> inventories,
        CancellationToken cancellationToken)
    {
        if (inventories.Count == 0)
        {
            return Array.Empty<FinishedGoodsInventoryDto>();
        }

        var specificationIds = inventories
            .Select(inventory => inventory.SpecificationId)
            .Distinct()
            .ToList();

        var warehouseIds = inventories
            .Select(inventory => inventory.WarehouseId)
            .Distinct()
            .ToList();

        var specifications = await context.Specifications
            .AsNoTracking()
            .Where(specification => specificationIds.Contains(specification.Id))
            .ToDictionaryAsync(specification => specification.Id, cancellationToken);

        var warehouses = await context.Warehouses
            .AsNoTracking()
            .Where(warehouse => warehouseIds.Contains(warehouse.Id))
            .ToDictionaryAsync(warehouse => warehouse.Id, cancellationToken);

        return inventories
            .Select(inventory => FinishedGoodsInventoryDto.FromEntity(
                inventory,
                specifications.TryGetValue(inventory.SpecificationId, out var specification) ? specification.Name : string.Empty,
                warehouses.TryGetValue(inventory.WarehouseId, out var warehouse) ? warehouse.Name : string.Empty))
            .ToList();
    }
}
