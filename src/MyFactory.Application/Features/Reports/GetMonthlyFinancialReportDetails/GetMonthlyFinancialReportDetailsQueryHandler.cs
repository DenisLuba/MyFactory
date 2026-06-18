using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Reports;

namespace MyFactory.Application.Features.Reports.GetMonthlyFinancialReportDetails;

public sealed class GetMonthlyFinancialReportDetailsQueryHandler
    : IRequestHandler<GetMonthlyFinancialReportDetailsQuery, MonthlyFinancialReportDetailsDto>
{
    private readonly IApplicationDbContext _db;

    public GetMonthlyFinancialReportDetailsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<MonthlyFinancialReportDetailsDto> Handle(
        GetMonthlyFinancialReportDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var report = await _db.MonthlyFinancialReports
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.ReportYear == request.Year && x.ReportMonth == request.Month,
                cancellationToken)
            ?? throw new NotFoundException("Monthly financial report not found");

        return new MonthlyFinancialReportDetailsDto(
            report.Id,
            report.ReportYear,
            report.ReportMonth,
            report.TotalRevenue,
            report.PayrollExpenses,
            report.MaterialExpenses,
            report.OtherExpenses,
            report.TotalRevenue - report.PayrollExpenses - report.MaterialExpenses - report.OtherExpenses,
            report.Status,
            report.CalculatedAt,
            report.CreatedBy);
    }
}
