using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Specifications;
using MyFactory.Domain.Entities.Specifications;

namespace MyFactory.Application.Features.Specifications.Queries.GetSpecificationCost;

public sealed class GetSpecificationCostQueryHandler : IRequestHandler<GetSpecificationCostQuery, SpecificationCostDto>
{
    private readonly IApplicationDbContext _context;

    public GetSpecificationCostQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SpecificationCostDto> Handle(GetSpecificationCostQuery request, CancellationToken cancellationToken)
    {
        var specification = await _context.Specifications
            .AsNoTracking()
            .Include(spec => spec.BomItems)
            .Include(spec => spec.Operations)
            .FirstOrDefaultAsync(spec => spec.Id == request.SpecificationId, cancellationToken)
            ?? throw new InvalidOperationException("Specification not found.");

        var asOfDate = request.AsOfDate ?? DateOnly.FromDateTime(DateTime.UtcNow);

        var materialsCost = await CalculateMaterialsCostAsync(specification, asOfDate, cancellationToken);
        var operationsCost = specification.Operations.Sum(operation => operation.OperationCost);

        var workshopExpenses = await _context.WorkshopExpenseHistoryEntries
            .AsNoTracking()
            .Where(entry => entry.SpecificationId == request.SpecificationId
                && entry.EffectiveFrom <= asOfDate
                && (entry.EffectiveTo == null || entry.EffectiveTo >= asOfDate))
            .SumAsync(entry => entry.AmountPerUnit, cancellationToken);

        var total = materialsCost + operationsCost + workshopExpenses;

        return new SpecificationCostDto(
            specification.Id,
            asOfDate,
            materialsCost,
            operationsCost,
            workshopExpenses,
            total);
    }

    private async Task<decimal> CalculateMaterialsCostAsync(Specification specification, DateOnly asOfDate, CancellationToken cancellationToken)
    {
        if (specification.BomItems.Count == 0)
        {
            return 0m;
        }

        var missingCostMaterialIds = specification.BomItems
            .Where(item => !item.UnitCost.HasValue)
            .Select(item => item.MaterialId)
            .Distinct()
            .ToArray();

        var fallbackPrices = await LoadLatestMaterialPricesAsync(missingCostMaterialIds, asOfDate, cancellationToken);

        decimal total = 0m;
        foreach (var item in specification.BomItems)
        {
            var unitCost = item.UnitCost ?? (fallbackPrices.TryGetValue(item.MaterialId, out var fallback) ? fallback : 0m);
            total += item.Quantity * unitCost;
        }

        return total;
    }

    private async Task<Dictionary<Guid, decimal>> LoadLatestMaterialPricesAsync(
        IReadOnlyCollection<Guid> materialIds,
        DateOnly asOfDate,
        CancellationToken cancellationToken)
    {
        if (materialIds.Count == 0)
        {
            return new Dictionary<Guid, decimal>();
        }

        var entries = await _context.MaterialPriceHistoryEntries
            .AsNoTracking()
            .Where(entry => materialIds.Contains(entry.MaterialId) && entry.EffectiveFrom <= asOfDate)
            .OrderBy(entry => entry.MaterialId)
            .ThenByDescending(entry => entry.EffectiveFrom)
            .ToListAsync(cancellationToken);

        return entries
            .GroupBy(entry => entry.MaterialId)
            .Where(group => group.Any())
            .ToDictionary(group => group.Key, group => group.First().Price);
    }
}
