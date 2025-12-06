using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Materials;

namespace MyFactory.Application.Features.Materials.Commands.AddMaterialPrice;

public sealed class AddMaterialPriceCommandHandler : IRequestHandler<AddMaterialPriceCommand, MaterialPriceHistoryDto>
{
    private readonly IApplicationDbContext _context;

    public AddMaterialPriceCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MaterialPriceHistoryDto> Handle(AddMaterialPriceCommand request, CancellationToken cancellationToken)
    {
        if (request.EffectiveTo.HasValue && request.EffectiveTo.Value < request.EffectiveFrom)
        {
            throw new InvalidOperationException("Effective to date cannot be earlier than effective from date.");
        }

        var material = await _context.Materials
            .FirstOrDefaultAsync(entity => entity.Id == request.MaterialId, cancellationToken);

        if (material is null)
        {
            throw new InvalidOperationException("Material not found.");
        }

        var supplier = await _context.Suppliers
            .FirstOrDefaultAsync(entity => entity.Id == request.SupplierId, cancellationToken);

        if (supplier is null)
        {
            throw new InvalidOperationException("Supplier not found.");
        }

        var newRangeEnd = request.EffectiveTo ?? DateTime.MaxValue;
        var overlaps = await _context.MaterialPriceHistoryEntries
            .AsNoTracking()
            .AnyAsync(entry => entry.MaterialId == request.MaterialId
                && entry.SupplierId == request.SupplierId
                && entry.EffectiveFrom <= newRangeEnd
                && (entry.EffectiveTo ?? DateTime.MaxValue) >= request.EffectiveFrom, cancellationToken);

        if (overlaps)
        {
            throw new InvalidOperationException("Material price history overlaps with an existing entry.");
        }

        var entry = material.AddPrice(request.SupplierId, request.Price, request.EffectiveFrom, request.DocRef.Trim());

        if (request.EffectiveTo.HasValue)
        {
            entry.SetEffectiveTo(request.EffectiveTo.Value);
        }

        await _context.MaterialPriceHistoryEntries.AddAsync(entry, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var supplierDto = SupplierDto.FromEntity(supplier);
        return MaterialPriceHistoryDto.FromEntity(entry, supplierDto);
    }
}
