using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Warehousing;

namespace MyFactory.Application.OldFeatures.Warehousing.Queries.GetInventoryReceiptById;

public sealed class GetInventoryReceiptByIdQueryHandler : IRequestHandler<GetInventoryReceiptByIdQuery, InventoryReceiptDto>
{
    private readonly IApplicationDbContext _context;

    public GetInventoryReceiptByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryReceiptDto> Handle(GetInventoryReceiptByIdQuery request, CancellationToken cancellationToken)
    {
        var receipt = await _context.InventoryReceipts
            .AsNoTracking()
            .Include(r => r.Items)
            .Include(r => r.Supplier)
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken)
            ?? throw new InvalidOperationException("Inventory receipt not found.");

        var materialIds = receipt.Items.Select(item => item.MaterialId).Distinct().ToList();
        var materials = await _context.Materials
            .Where(material => materialIds.Contains(material.Id))
            .ToDictionaryAsync(material => material.Id, cancellationToken);

        if (receipt.Supplier is null)
        {
            throw new InvalidOperationException("Supplier data is required for receipt.");
        }

        return InventoryReceiptDto.FromEntity(receipt, receipt.Supplier, receipt.Items, materials);
    }
}
