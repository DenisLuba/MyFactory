using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.OldFeatures.Warehousing.Queries.GetInventoryItem;

public sealed class GetInventoryItemQueryHandler : IRequestHandler<GetInventoryItemQuery, InventoryItemDto>
{
    private readonly IApplicationDbContext _context;

    public GetInventoryItemQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryItemDto> Handle(GetInventoryItemQuery request, CancellationToken cancellationToken)
    {
        var inventoryItem = await _context.InventoryItems
            .AsNoTracking()
            .Include(item => item.Material)
            .FirstOrDefaultAsync(item => item.WarehouseId == request.WarehouseId && item.MaterialId == request.MaterialId, cancellationToken)
            ?? throw new InvalidOperationException("Inventory item not found.");

        var material = inventoryItem.Material
            ?? await _context.Materials
                .FirstOrDefaultAsync(material => material.Id == inventoryItem.MaterialId, cancellationToken)
                ?? throw new InvalidOperationException("Material not found.");

        return InventoryItemDto.FromEntity(inventoryItem, material);
    }
}
