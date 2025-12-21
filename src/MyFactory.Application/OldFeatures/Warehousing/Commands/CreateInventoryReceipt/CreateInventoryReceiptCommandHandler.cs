using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Warehousing;
using MyFactory.Domain.Entities.Warehousing;

namespace MyFactory.Application.OldFeatures.Warehousing.Commands.CreateInventoryReceipt;

public sealed class CreateInventoryReceiptCommandHandler : IRequestHandler<CreateInventoryReceiptCommand, InventoryReceiptDto>
{
    private readonly IApplicationDbContext _context;

    public CreateInventoryReceiptCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryReceiptDto> Handle(CreateInventoryReceiptCommand request, CancellationToken cancellationToken)
    {
        var receiptNumber = request.ReceiptNumber.Trim();

        var exists = await _context.InventoryReceipts
            .AsNoTracking()
            .AnyAsync(receipt => receipt.ReceiptNumber == receiptNumber, cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException("Receipt number already exists.");
        }

        var supplier = await _context.Suppliers
            .FirstOrDefaultAsync(entity => entity.Id == request.SupplierId, cancellationToken)
            ?? throw new InvalidOperationException("Supplier not found.");

        var receipt = new InventoryReceipt(receiptNumber, request.SupplierId, request.ReceiptDate);
        await _context.InventoryReceipts.AddAsync(receipt, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return InventoryReceiptDto.FromEntity(receipt, supplier, Array.Empty<InventoryReceiptItemDto>());
    }
}
