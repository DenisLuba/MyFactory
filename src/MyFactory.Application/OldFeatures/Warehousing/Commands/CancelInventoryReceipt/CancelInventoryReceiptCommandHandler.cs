using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Warehousing;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.OldFeatures.Warehousing.Commands.CancelInventoryReceipt;

public sealed class CancelInventoryReceiptCommandHandler : IRequestHandler<CancelInventoryReceiptCommand, InventoryReceiptDto>
{
    private readonly IApplicationDbContext _context;

    public CancelInventoryReceiptCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryReceiptDto> Handle(CancelInventoryReceiptCommand request, CancellationToken cancellationToken)
    {
        var receipt = await _context.InventoryReceipts
            .Include(r => r.Items)
            .FirstOrDefaultAsync(r => r.Id == request.ReceiptId, cancellationToken)
            ?? throw new InvalidOperationException("Inventory receipt not found.");

        if (receipt.Status != InventoryReceiptStatuses.Draft)
        {
            throw new InvalidOperationException("Only draft receipts can be cancelled.");
        }

        receipt.Cancel();
        await _context.SaveChangesAsync(cancellationToken);

        var materialIds = receipt.Items.Select(item => item.MaterialId).Distinct().ToList();
        var materials = await _context.Materials
            .Where(material => materialIds.Contains(material.Id))
            .ToDictionaryAsync(material => material.Id, cancellationToken);

        var supplier = await _context.Suppliers
            .FirstOrDefaultAsync(entity => entity.Id == receipt.SupplierId, cancellationToken)
            ?? throw new InvalidOperationException("Supplier not found.");

        return InventoryReceiptDto.FromEntity(receipt, supplier, receipt.Items, materials);
    }
}
