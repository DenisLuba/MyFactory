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

namespace MyFactory.Application.Features.Warehousing.Commands.AddInventoryReceiptItem;

public sealed class AddInventoryReceiptItemCommandHandler : IRequestHandler<AddInventoryReceiptItemCommand, InventoryReceiptDto>
{
    private readonly IApplicationDbContext _context;

    public AddInventoryReceiptItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryReceiptDto> Handle(AddInventoryReceiptItemCommand request, CancellationToken cancellationToken)
    {
        var receipt = await _context.InventoryReceipts
            .Include(r => r.Items)
            .FirstOrDefaultAsync(r => r.Id == request.ReceiptId, cancellationToken)
            ?? throw new InvalidOperationException("Inventory receipt not found.");

        if (receipt.Status != InventoryReceiptStatus.Draft)
        {
            throw new InvalidOperationException("Only draft receipts can be edited.");
        }

        var material = await _context.Materials
            .FirstOrDefaultAsync(entity => entity.Id == request.MaterialId, cancellationToken)
            ?? throw new InvalidOperationException("Material not found.");

        var newItem = receipt.AddItem(request.MaterialId, request.Quantity, request.UnitPrice);
        await _context.InventoryReceiptItems.AddAsync(newItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var materialIds = receipt.Items.Select(item => item.MaterialId).Distinct().ToList();
        var materialLookup = await _context.Materials
            .Where(entity => materialIds.Contains(entity.Id))
            .ToDictionaryAsync(entity => entity.Id, cancellationToken);

        var supplier = await _context.Suppliers
            .FirstOrDefaultAsync(entity => entity.Id == receipt.SupplierId, cancellationToken)
            ?? throw new InvalidOperationException("Supplier not found.");

        return InventoryReceiptDto.FromEntity(receipt, supplier, receipt.Items, materialLookup);
    }
}
