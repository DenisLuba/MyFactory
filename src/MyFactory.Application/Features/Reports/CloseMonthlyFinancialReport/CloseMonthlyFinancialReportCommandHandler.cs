using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.Reports.CloseMonthlyFinancialReport;

public sealed class CloseMonthlyFinancialReportCommandHandler
    : IRequestHandler<CloseMonthlyFinancialReportCommand>
{
    private readonly IApplicationDbContext _db;

    public CloseMonthlyFinancialReportCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(CloseMonthlyFinancialReportCommand request, CancellationToken cancellationToken)
    {
        var report = await _db.MonthlyFinancialReports
            .FirstOrDefaultAsync(
                x => x.ReportYear == request.Year && x.ReportMonth == request.Month,
                cancellationToken)
            ?? throw new NotFoundException("Monthly financial report not found");

        report.Close();

        await _db.SaveChangesAsync(cancellationToken);
    }
}
