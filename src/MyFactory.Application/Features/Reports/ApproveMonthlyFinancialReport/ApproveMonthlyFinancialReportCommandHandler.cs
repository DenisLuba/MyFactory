using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Finance;
using MyFactory.Domain.Exceptions;

namespace MyFactory.Application.Features.Reports.ApproveMonthlyFinancialReport;

public sealed class ApproveMonthlyFinancialReportCommandHandler
    : IRequestHandler<ApproveMonthlyFinancialReportCommand>
{
    private readonly IApplicationDbContext _db;

    public ApproveMonthlyFinancialReportCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(ApproveMonthlyFinancialReportCommand request, CancellationToken cancellationToken)
    {
        var report = await _db.MonthlyFinancialReports
            .FirstOrDefaultAsync(
                x => x.ReportYear == request.Year && x.ReportMonth == request.Month,
                cancellationToken)
            ?? throw new NotFoundException("Monthly financial report not found");

        report.Approve();

        await _db.SaveChangesAsync(cancellationToken);
    }
}
