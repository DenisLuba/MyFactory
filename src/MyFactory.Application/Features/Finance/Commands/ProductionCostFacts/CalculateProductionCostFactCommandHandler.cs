using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Finance;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.Features.Finance.Commands.ProductionCostFacts;

public sealed class CalculateProductionCostFactCommandHandler : IRequestHandler<CalculateProductionCostFactCommand, ProductionCostFactDto>
{
    private readonly IApplicationDbContext _context;

    public CalculateProductionCostFactCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductionCostFactDto> Handle(CalculateProductionCostFactCommand request, CancellationToken cancellationToken)
    {
        var specification = await _context.Specifications
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == request.SpecificationId, cancellationToken)
            ?? throw new InvalidOperationException("Specification not found.");

        var periodOverhead = await _context.OverheadMonthlyEntries
            .Where(entity => entity.PeriodMonth == request.PeriodMonth && entity.PeriodYear == request.PeriodYear)
            .SumAsync(entity => entity.Amount, cancellationToken);

        var fact = await _context.ProductionCostFacts
            .FirstOrDefaultAsync(
                entity => entity.PeriodMonth == request.PeriodMonth
                    && entity.PeriodYear == request.PeriodYear
                    && entity.SpecificationId == request.SpecificationId,
                cancellationToken);

        if (fact is null)
        {
            fact = new ProductionCostFact(
                request.PeriodMonth,
                request.PeriodYear,
                request.SpecificationId,
                request.QuantityProduced,
                request.MaterialCost,
                request.LaborCost,
                periodOverhead);

            await _context.ProductionCostFacts.AddAsync(fact, cancellationToken);
        }
        else
        {
            fact.UpdateQuantity(request.QuantityProduced);
            fact.UpdateCosts(request.MaterialCost, request.LaborCost, periodOverhead);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return ProductionCostFactDto.FromEntity(fact, specification.Name);
    }
}
