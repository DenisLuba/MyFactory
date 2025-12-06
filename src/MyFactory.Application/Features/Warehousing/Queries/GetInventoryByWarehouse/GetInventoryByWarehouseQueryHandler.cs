using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.Features.Warehousing.Queries.GetInventoryByWarehouse;

public sealed class GetInventoryByWarehouseQueryHandler : IRequestHandler<GetInventoryByWarehouseQuery, IReadOnlyCollection<InventoryItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetInventoryByWarehouseQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<InventoryItemDto>> Handle(GetInventoryByWarehouseQuery request, CancellationToken cancellationToken)
    {
        var warehouseExists = await _context.Warehouses
            .AsNoTracking()
            .AnyAsync(warehouse => warehouse.Id == request.WarehouseId, cancellationToken);

        if (!warehouseExists)
        {
            throw new InvalidOperationException("Warehouse not found.");
        }

        var inventoryItems = await _context.InventoryItems
            .AsNoTracking()
            .Where(item => item.WarehouseId == request.WarehouseId)
            .Include(item => item.Material)
            .ToListAsync(cancellationToken);

        var materialIds = inventoryItems.Where(item => item.Material is null).Select(item => item.MaterialId).Distinct().ToList();
        var missingMaterials = materialIds.Count > 0
            ? await _context.Materials
                .Where(material => materialIds.Contains(material.Id))
                .ToDictionaryAsync(material => material.Id, cancellationToken)
            : new Dictionary<Guid, MyFactory.Domain.Entities.Materials.Material>();

        return inventoryItems
            .Select(item =>
            {
                var material = item.Material;

                if (material is null)
                {
                    if (!missingMaterials.TryGetValue(item.MaterialId, out material))
                    {
                        throw new InvalidOperationException("Material not found for inventory item.");
                    }
                }

                return InventoryItemDto.FromEntity(item, material);
            })
            .ToList();
    }
}
