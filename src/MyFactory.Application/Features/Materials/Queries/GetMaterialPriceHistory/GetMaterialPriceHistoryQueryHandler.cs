using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Materials;

namespace MyFactory.Application.Features.Materials.Queries.GetMaterialPriceHistory;

public sealed class GetMaterialPriceHistoryQueryHandler : IRequestHandler<GetMaterialPriceHistoryQuery, IReadOnlyCollection<MaterialPriceHistoryDto>>
{
    private readonly IApplicationDbContext _context;

    public GetMaterialPriceHistoryQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<MaterialPriceHistoryDto>> Handle(GetMaterialPriceHistoryQuery request, CancellationToken cancellationToken)
    {
        var materialExists = await _context.Materials
            .AsNoTracking()
            .AnyAsync(material => material.Id == request.MaterialId, cancellationToken);

        if (!materialExists)
        {
            throw new InvalidOperationException("Material not found.");
        }

        var query = _context.MaterialPriceHistoryEntries
            .AsNoTracking()
            .Where(entry => entry.MaterialId == request.MaterialId);

        if (request.SupplierId.HasValue)
        {
            query = query.Where(entry => entry.SupplierId == request.SupplierId.Value);
        }

        var entries = await query
            .Include(entry => entry.Supplier)
            .OrderByDescending(entry => entry.EffectiveFrom)
            .ToListAsync(cancellationToken);

        return entries
            .Select(entry =>
            {
                var supplierDto = entry.Supplier is not null
                    ? SupplierDto.FromEntity(entry.Supplier)
                    : new SupplierDto(entry.SupplierId, string.Empty, string.Empty, false);

                return MaterialPriceHistoryDto.FromEntity(entry, supplierDto);
            })
            .ToList();
    }
}
