using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Warehousing;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.OldFeatures.Warehousing.Commands.PostInventoryReceipt;

public sealed class PostInventoryReceiptCommandHandler : IRequestHandler<PostInventoryReceiptCommand, InventoryReceiptDto>
{
    private readonly IApplicationDbContext _context;

    public PostInventoryReceiptCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryReceiptDto> Handle(PostInventoryReceiptCommand request, CancellationToken cancellationToken)
    {
        var receipt = await _context.InventoryReceipts
            .Include(r => r.Items)
            .FirstOrDefaultAsync(r => r.Id == request.ReceiptId, cancellationToken)
            ?? throw new InvalidOperationException("Inventory receipt not found.");

        if (receipt.Status != InventoryReceiptStatuses.Draft)
        {
            throw new InvalidOperationException("Only draft receipts can be posted.");
        }

        if (!receipt.Items.Any())
        {
            throw new InvalidOperationException("Receipt has no items to post.");
        }

        var warehouseExists = await _context.Warehouses
            .AsNoTracking()
            .AnyAsync(warehouse => warehouse.Id == request.WarehouseId, cancellationToken);

        if (!warehouseExists)
        {
            throw new InvalidOperationException("Warehouse not found.");
        }

        var materialIds = receipt.Items.Select(item => item.MaterialId).Distinct().ToList();
        var materials = await _context.Materials
            .Where(material => materialIds.Contains(material.Id))
            .ToDictionaryAsync(material => material.Id, cancellationToken);

        if (materials.Count != materialIds.Count)
        {
            throw new InvalidOperationException("One or more materials referenced by receipt items do not exist.");
        }

        var existingInventory = await _context.InventoryItems
            .Where(item => item.WarehouseId == request.WarehouseId && materialIds.Contains(item.MaterialId))
            .ToDictionaryAsync(item => item.MaterialId, cancellationToken);

        foreach (var item in receipt.Items)
        {
            if (!existingInventory.TryGetValue(item.MaterialId, out var inventoryItem))
            {
                inventoryItem = new InventoryItem(request.WarehouseId, item.MaterialId);
                await _context.InventoryItems.AddAsync(inventoryItem, cancellationToken);
                existingInventory[item.MaterialId] = inventoryItem;
            }

            inventoryItem.Receive(item.Quantity, item.UnitPrice);
        }

        receipt.MarkAsReceived();
        await _context.SaveChangesAsync(cancellationToken);

        var supplier = await _context.Suppliers
            .FirstOrDefaultAsync(entity => entity.Id == receipt.SupplierId, cancellationToken)
            ?? throw new InvalidOperationException("Supplier not found.");

        return InventoryReceiptDto.FromEntity(receipt, supplier, receipt.Items, materials);
    }
}
