using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Reports;

namespace MyFactory.Application.Features.Reports.GetMonthlyFinancialReports;

public sealed class GetMonthlyFinancialReportsQueryHandler
    : IRequestHandler<GetMonthlyFinancialReportsQuery, IReadOnlyList<MonthlyFinancialReportListItemDto>>
{
    private readonly IApplicationDbContext _db;

    public GetMonthlyFinancialReportsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<MonthlyFinancialReportListItemDto>> Handle(
        GetMonthlyFinancialReportsQuery request,
        CancellationToken cancellationToken)
    {
        var reports = await _db.MonthlyFinancialReports
            .AsNoTracking()
            .OrderByDescending(x => x.ReportYear)
            .ThenByDescending(x => x.ReportMonth)
            .Select(x => new MonthlyFinancialReportListItemDto(
                x.ReportYear,
                x.ReportMonth,
                x.TotalRevenue,
                x.PayrollExpenses,
                x.MaterialExpenses,
                x.OtherExpenses,
                x.TotalRevenue - x.PayrollExpenses - x.MaterialExpenses - x.OtherExpenses,
                x.Status))
            .ToListAsync(cancellationToken);

        return reports;
    }
}
