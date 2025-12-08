using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Finance;

namespace MyFactory.Application.Features.Finance.Queries.GetRevenueReportsByPeriod;

public sealed class GetRevenueReportsByPeriodQueryHandler : IRequestHandler<GetRevenueReportsByPeriodQuery, IReadOnlyCollection<RevenueReportDto>>
{
    private readonly IApplicationDbContext _context;

    public GetRevenueReportsByPeriodQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<RevenueReportDto>> Handle(GetRevenueReportsByPeriodQuery request, CancellationToken cancellationToken)
    {
        var reports = await _context.RevenueReports
            .AsNoTracking()
            .Include(entity => entity.Specification)
            .Where(entity => entity.PeriodMonth == request.PeriodMonth && entity.PeriodYear == request.PeriodYear)
            .OrderBy(entity => entity.Specification!.Name)
            .ToListAsync(cancellationToken);

        return reports
            .Select(entity => RevenueReportDto.FromEntity(entity, entity.Specification?.Name ?? string.Empty))
            .ToList();
    }
}
