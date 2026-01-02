using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Finance;
using MyFactory.Domain.Entities.Inventory;
using MyFactory.Domain.Entities.Orders;

namespace MyFactory.Application.Features.Reports.CalculateMonthlyFinancialReport;

public sealed class CalculateMonthlyFinancialReportCommandHandler
    : IRequestHandler<CalculateMonthlyFinancialReportCommand, Guid>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public CalculateMonthlyFinancialReportCommandHandler(
        IApplicationDbContext db,
        ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(
        CalculateMonthlyFinancialReportCommand request,
        CancellationToken cancellationToken)
    {
        var existing = await _db.MonthlyFinancialReports
            .FirstOrDefaultAsync(
                x => x.ReportYear == request.Year && x.ReportMonth == request.Month,
                cancellationToken);

        if (existing != null && existing.Status == MonthlyReportStatus.Closed)
            throw new DomainException("Monthly report is closed and cannot be recalculated.");

        var (totalRevenue, payroll, materials, other) = await CalculateAggregates(request.Year, request.Month, cancellationToken);
        var profit = totalRevenue - payroll - materials - other;

        if (existing == null)
        {
            var report = new MonthlyFinancialReportEntity(
                request.Year,
                request.Month,
                totalRevenue,
                payroll,
                materials,
                other,
                MonthlyReportStatus.Calculated,
                DateTime.UtcNow,
                _currentUser.UserId);

            _db.MonthlyFinancialReports.Add(report);
            await _db.SaveChangesAsync(cancellationToken);
            return report.Id;
        }

        existing.Recalculate(totalRevenue, payroll, materials, other);

        await _db.SaveChangesAsync(cancellationToken);
        return existing.Id;
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
            .ToDictionaryAsync(key => key.ProductId, value => value.CostPerUnit, cancellationToken);

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
