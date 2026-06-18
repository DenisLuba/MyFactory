using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Finance;
using MyFactory.Domain.Entities.Inventory;
using MyFactory.Domain.Entities.Orders;

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
            throw new DomainApplicationException("Monthly report is closed and cannot be recalculated.");

        var (revenue, payroll, materials, other) = await CalculateAggregates(request.Year, request.Month, cancellationToken);

        report.Recalculate(revenue, payroll, materials, other);

        await _db.SaveChangesAsync(cancellationToken);
    }

    private async Task<(decimal revenue, decimal payroll, decimal materials, decimal other)> CalculateAggregates(
        int year,
        int month,
        CancellationToken cancellationToken)
    {
        var from = new DateOnly(year, month, 1);
        var to = from.AddMonths(1);
        var fromDateTime = from.ToDateTime(TimeOnly.MinValue);
        var toDateTime = to.ToDateTime(TimeOnly.MinValue);

        var payroll = await _db.PayrollAccruals
            .AsNoTracking()
            .Where(x => x.AccrualDate >= from && x.AccrualDate < to)
            .SumAsync(x => (decimal?)x.TotalAmount, cancellationToken) ?? 0m;

        var otherExpenses = await _db.Expenses
            .AsNoTracking()
            .Where(x => x.ExpenseDate >= from && x.ExpenseDate < to)
            .SumAsync(x => (decimal?)x.Amount, cancellationToken) ?? 0m;

        otherExpenses += await _db.CashAdvanceExpenses
            .AsNoTracking()
            .Where(x => x.ExpenseDate >= from && x.ExpenseDate < to)
            .SumAsync(x => (decimal?)x.Amount, cancellationToken) ?? 0m;

        var productOverheadByProduct = await _db.ProductDepartmentCosts
            .AsNoTracking()
            .Where(x => x.IsActive)
            .GroupBy(x => x.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                CostPerUnit = g.Sum(x => x.ExpensesPerUnit + x.CutCostPerUnit + x.SewingCostPerUnit + x.PackCostPerUnit)
            })
            .ToDictionaryAsync(x => x.ProductId, x => x.CostPerUnit, cancellationToken);

        var shippedItems = await (
                from s in _db.Shipments.AsNoTracking()
                join si in _db.ShipmentItems.AsNoTracking() on s.Id equals si.ShipmentId
                where s.Status == ShipmentStatus.Shipped
                      && s.ShipmentDate >= fromDateTime
                      && s.ShipmentDate < toDateTime
                select new
                {
                    si.ProductId,
                    si.Qty,
                    Revenue = si.Qty * si.UnitPrice
                })
            .ToListAsync(cancellationToken);

        var totalRevenue = shippedItems.Sum(x => x.Revenue);

        var productionOverhead = shippedItems.Sum(x =>
        {
            return productOverheadByProduct.TryGetValue(x.ProductId, out var cost)
                ? x.Qty * cost
                : 0m;
        });

        otherExpenses += productionOverhead;

        var materialExpenses = await (
                from i in _db.InventoryMovementItems.AsNoTracking()
                join m in _db.InventoryMovements.AsNoTracking() on i.MovementId equals m.Id
                where m.CreatedAt >= fromDateTime && m.CreatedAt < toDateTime
                      && (m.MovementType == InventoryMovementType.IssueToDept || m.MovementType == InventoryMovementType.ReturnFromDept)
                select (decimal?)i.Qty * i.UnitCost * (m.MovementType == InventoryMovementType.ReturnFromDept ? -1 : 1))
            .SumAsync(cancellationToken) ?? 0m;

        return (totalRevenue, payroll, materialExpenses, otherExpenses);
    }
}
