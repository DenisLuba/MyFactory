using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.Features.Warehousing.Commands.ReserveInventory;

public sealed class ReserveInventoryCommandHandler : IRequestHandler<ReserveInventoryCommand, InventoryItemDto>
{
    private readonly IApplicationDbContext _context;

    public ReserveInventoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryItemDto> Handle(ReserveInventoryCommand request, CancellationToken cancellationToken)
    {
        var inventoryItem = await _context.InventoryItems
            .Include(item => item.Material)
            .FirstOrDefaultAsync(item => item.WarehouseId == request.WarehouseId && item.MaterialId == request.MaterialId, cancellationToken)
            ?? throw new InvalidOperationException("Inventory item not found.");

        inventoryItem.Reserve(request.Quantity);
        await _context.SaveChangesAsync(cancellationToken);

        var material = inventoryItem.Material
            ?? await _context.Materials
                .FirstOrDefaultAsync(material => material.Id == inventoryItem.MaterialId, cancellationToken)
                ?? throw new InvalidOperationException("Material not found.");

        return InventoryItemDto.FromEntity(inventoryItem, material);
    }
}
