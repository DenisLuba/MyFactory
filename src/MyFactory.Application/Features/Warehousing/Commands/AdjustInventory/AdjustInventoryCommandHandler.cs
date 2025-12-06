using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Warehousing;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.Features.Warehousing.Commands.AdjustInventory;

public sealed class AdjustInventoryCommandHandler : IRequestHandler<AdjustInventoryCommand, InventoryItemDto>
{
    private readonly IApplicationDbContext _context;

    public AdjustInventoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryItemDto> Handle(AdjustInventoryCommand request, CancellationToken cancellationToken)
    {
        if (request.QuantityDelta == 0)
        {
            throw new InvalidOperationException("Quantity delta must be non-zero.");
        }

        var warehouseExists = await _context.Warehouses
            .AsNoTracking()
            .AnyAsync(warehouse => warehouse.Id == request.WarehouseId, cancellationToken);

        if (!warehouseExists)
        {
            throw new InvalidOperationException("Warehouse not found.");
        }

        var material = await _context.Materials
            .FirstOrDefaultAsync(entity => entity.Id == request.MaterialId, cancellationToken)
            ?? throw new InvalidOperationException("Material not found.");

        var inventoryItem = await _context.InventoryItems
            .FirstOrDefaultAsync(item => item.WarehouseId == request.WarehouseId && item.MaterialId == request.MaterialId, cancellationToken);

        if (inventoryItem is null)
        {
            if (request.QuantityDelta < 0)
            {
                throw new InvalidOperationException("Cannot reduce inventory that does not exist.");
            }

            inventoryItem = new InventoryItem(request.WarehouseId, request.MaterialId);
            await _context.InventoryItems.AddAsync(inventoryItem, cancellationToken);
        }

        if (request.QuantityDelta > 0)
        {
            if (!request.UnitPrice.HasValue || request.UnitPrice.Value <= 0)
            {
                throw new InvalidOperationException("Unit price must be provided and positive for positive adjustments.");
            }

            inventoryItem.Receive(request.QuantityDelta, request.UnitPrice.Value);
        }
        else
        {
            inventoryItem.Issue(Math.Abs(request.QuantityDelta));
        }

        await _context.SaveChangesAsync(cancellationToken);
        return InventoryItemDto.FromEntity(inventoryItem, material);
    }
}
