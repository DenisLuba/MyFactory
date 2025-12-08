using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Advances;

namespace MyFactory.Application.Features.Advances.Queries.GetAdvanceReports;

public sealed class GetAdvanceReportsQueryHandler : IRequestHandler<GetAdvanceReportsQuery, IReadOnlyCollection<AdvanceReportDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAdvanceReportsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<AdvanceReportDto>> Handle(GetAdvanceReportsQuery request, CancellationToken cancellationToken)
    {
        var advanceExists = await _context.Advances
            .AsNoTracking()
            .AnyAsync(entity => entity.Id == request.AdvanceId, cancellationToken);

        if (!advanceExists)
        {
            throw new InvalidOperationException("Advance not found.");
        }

        var reports = await _context.AdvanceReports
            .AsNoTracking()
            .Where(report => report.AdvanceId == request.AdvanceId)
            .OrderBy(report => report.ReportedAt)
            .ToListAsync(cancellationToken);

        return reports
            .Select(AdvanceReportDto.FromEntity)
            .ToArray();
    }
}
