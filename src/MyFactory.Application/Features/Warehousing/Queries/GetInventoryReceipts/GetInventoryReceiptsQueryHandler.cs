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

namespace MyFactory.Application.Features.Warehousing.Queries.GetInventoryReceipts;

public sealed class GetInventoryReceiptsQueryHandler : IRequestHandler<GetInventoryReceiptsQuery, IReadOnlyCollection<InventoryReceiptDto>>
{
    private readonly IApplicationDbContext _context;

    public GetInventoryReceiptsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<InventoryReceiptDto>> Handle(GetInventoryReceiptsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.InventoryReceipts
            .AsNoTracking()
            .Include(receipt => receipt.Items)
            .Include(receipt => receipt.Supplier)
            .AsQueryable();

        if (request.SupplierId.HasValue)
        {
            query = query.Where(receipt => receipt.SupplierId == request.SupplierId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            var status = request.Status.Trim();
            query = query.Where(receipt => receipt.Status == status);
        }

        var receipts = await query
            .OrderByDescending(receipt => receipt.ReceiptDate)
            .ToListAsync(cancellationToken);

        var materialIds = receipts
            .SelectMany(receipt => receipt.Items)
            .Select(item => item.MaterialId)
            .Distinct()
            .ToList();

        var materials = await _context.Materials
            .Where(material => materialIds.Contains(material.Id))
            .ToDictionaryAsync(material => material.Id, cancellationToken);

        return receipts
            .Select(receipt =>
            {
                if (receipt.Supplier is null)
                {
                    throw new InvalidOperationException("Supplier data is required for receipt.");
                }

                return InventoryReceiptDto.FromEntity(receipt, receipt.Supplier, receipt.Items, materials);
            })
            .ToList();
    }
}
