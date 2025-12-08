using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Finance;

namespace MyFactory.Application.Features.Finance.Commands.RevenueReports;

public sealed class MarkRevenuePaidCommandHandler : IRequestHandler<MarkRevenuePaidCommand, RevenueReportDto>
{
    private readonly IApplicationDbContext _context;

    public MarkRevenuePaidCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RevenueReportDto> Handle(MarkRevenuePaidCommand request, CancellationToken cancellationToken)
    {
        var report = await _context.RevenueReports
            .Include(entity => entity.Specification)
            .FirstOrDefaultAsync(entity => entity.Id == request.RevenueReportId, cancellationToken)
            ?? throw new InvalidOperationException("Revenue report not found.");

        report.MarkPaid(request.PaymentDate);

        await _context.SaveChangesAsync(cancellationToken);

        return RevenueReportDto.FromEntity(report, report.Specification?.Name ?? string.Empty);
    }
}
