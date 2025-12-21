using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Inventory;
using System.Linq;

namespace MyFactory.Application.OldFeatures.Inventory.Queries.GetWarehouseMaterialInventory;

public sealed class GetWarehouseMaterialInventoryQueryHandler : IRequestHandler<GetWarehouseMaterialInventoryQuery, InventoryWarehouseMaterialDto>
{
    private readonly IApplicationDbContext _context;

    public GetWarehouseMaterialInventoryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryWarehouseMaterialDto> Handle(GetWarehouseMaterialInventoryQuery request, CancellationToken cancellationToken)
    {
        var inventoryProjection = await (
            from item in _context.InventoryItems.AsNoTracking()
            where item.WarehouseId == request.WarehouseId && item.MaterialId == request.MaterialId
            join material in _context.Materials.AsNoTracking() on item.MaterialId equals material.Id
            join warehouse in _context.Warehouses.AsNoTracking() on item.WarehouseId equals warehouse.Id
            select new InventoryWarehouseMaterialDto(
                material.Id,
                material.Name,
                material.Unit,
                warehouse.Id,
                warehouse.Name,
                warehouse.Type,
                warehouse.Location,
                item.Quantity,
                item.ReservedQuantity,
                item.AvailableQuantity,
                item.AveragePrice,
                item.Quantity * item.AveragePrice)
        ).FirstOrDefaultAsync(cancellationToken);

        if (inventoryProjection is null)
        {
            throw new InvalidOperationException("Inventory item not found for the specified warehouse and material.");
        }

        return inventoryProjection;
    }
}
