using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Finance;
using MonthlyProfitAggregate = MyFactory.Domain.Entities.Reports.MonthlyProfit;

namespace MyFactory.Application.OldFeatures.Finance.Commands.MonthlyProfit;

public sealed class CalculateMonthlyProfitCommandHandler : IRequestHandler<CalculateMonthlyProfitCommand, MonthlyProfitDto>
{
    private readonly IApplicationDbContext _context;

    public CalculateMonthlyProfitCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MonthlyProfitDto> Handle(CalculateMonthlyProfitCommand request, CancellationToken cancellationToken)
    {
        var revenue = await _context.RevenueReports
            .Where(entity => entity.PeriodMonth == request.PeriodMonth && entity.PeriodYear == request.PeriodYear)
            .SumAsync(entity => entity.TotalRevenue, cancellationToken);

        var productionCost = await _context.ProductionCostFacts
            .Where(entity => entity.PeriodMonth == request.PeriodMonth && entity.PeriodYear == request.PeriodYear)
            .SumAsync(entity => entity.TotalCost, cancellationToken);

        var overhead = await _context.OverheadMonthlyEntries
            .Where(entity => entity.PeriodMonth == request.PeriodMonth && entity.PeriodYear == request.PeriodYear)
            .SumAsync(entity => entity.Amount, cancellationToken);

        var monthlyProfit = await _context.MonthlyProfits
            .FirstOrDefaultAsync(
                entity => entity.PeriodMonth == request.PeriodMonth && entity.PeriodYear == request.PeriodYear,
                cancellationToken);

        if (monthlyProfit is null)
        {
            monthlyProfit = new MonthlyProfitAggregate(request.PeriodMonth, request.PeriodYear, revenue, productionCost, overhead);
            await _context.MonthlyProfits.AddAsync(monthlyProfit, cancellationToken);
        }
        else
        {
            monthlyProfit.UpdateFigures(revenue, productionCost, overhead);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return MonthlyProfitDto.FromEntity(monthlyProfit);
    }
}
