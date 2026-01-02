using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.Features.Reports.RecalculateMonthlyFinancialReport;

public sealed class RecalculateMonthlyFinancialReportCommandHandler
    : IRequestHandler<RecalculateMonthlyFinancialReportCommand>
{
    private readonly IApplicationDbContext _db;

    public RecalculateMonthlyFinancialReportCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(RecalculateMonthlyFinancialReportCommand request, CancellationToken cancellationToken)
    {
        var report = await _db.MonthlyFinancialReports
            .FirstOrDefaultAsync(
                x => x.ReportYear == request.Year && x.ReportMonth == request.Month,
                cancellationToken)
            ?? throw new NotFoundException("Monthly financial report not found");

        if (report.Status == MonthlyReportStatus.Closed)
            throw new DomainException("Monthly report is closed and cannot be recalculated.");

        var from = new DateOnly(request.Year, request.Month, 1);
        var to = from.AddMonths(1);

        var payroll = await _db.PayrollAccruals
            .AsNoTracking()
            .Where(x => x.AccrualDate >= from && x.AccrualDate < to)
            .SumAsync(x => (decimal?)x.TotalAmount, cancellationToken) ?? 0m;

        var other = await _db.Expenses
            .AsNoTracking()
            .Where(x => x.ExpenseDate >= from && x.ExpenseDate < to)
            .SumAsync(x => (decimal?)x.Amount, cancellationToken) ?? 0m;

        other += await _db.CashAdvanceExpenses
            .AsNoTracking()
            .Where(x => x.ExpenseDate >= from && x.ExpenseDate < to)
            .SumAsync(x => (decimal?)x.Amount, cancellationToken) ?? 0m;

        var materials = 0m;
        var revenue = 0m;

        report.Recalculate(revenue, payroll, materials, other);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
