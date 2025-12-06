using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Warehousing;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.Features.Warehousing.Commands.AddInventoryItem;

public sealed class AddInventoryItemCommandHandler : IRequestHandler<AddInventoryItemCommand, InventoryItemDto>
{
    private readonly IApplicationDbContext _context;

    public AddInventoryItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryItemDto> Handle(AddInventoryItemCommand request, CancellationToken cancellationToken)
    {
        var warehouseExists = await _context.Warehouses
            .AsNoTracking()
            .AnyAsync(warehouse => warehouse.Id == request.WarehouseId, cancellationToken);

        if (!warehouseExists)
        {
            throw new InvalidOperationException("Warehouse not found.");
        }

        var material = await _context.Materials
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == request.MaterialId, cancellationToken)
            ?? throw new InvalidOperationException("Material not found.");

        var exists = await _context.InventoryItems
            .AsNoTracking()
            .AnyAsync(item => item.WarehouseId == request.WarehouseId && item.MaterialId == request.MaterialId, cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException("Inventory item already exists for this material in the warehouse.");
        }

        var inventoryItem = new InventoryItem(request.WarehouseId, request.MaterialId);
        await _context.InventoryItems.AddAsync(inventoryItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return InventoryItemDto.FromEntity(inventoryItem, material);
    }
}
