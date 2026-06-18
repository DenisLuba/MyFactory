using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.Features.Finance.CalculateDailyPayrollAccrual;

public sealed class CalculateDailyPayrollAccrualCommandHandler
    : IRequestHandler<CalculateDailyPayrollAccrualCommand>
{
    private readonly IApplicationDbContext _db;

    public CalculateDailyPayrollAccrualCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        CalculateDailyPayrollAccrualCommand request,
        CancellationToken cancellationToken)
    {
        var employee =
            await _db.Employees
                .FirstOrDefaultAsync(x => x.Id == request.EmployeeId, cancellationToken)
            ?? throw new NotFoundException("Employee not found");

        var sewingOps = await (
            from op in _db.SewingOperations.AsNoTracking()
            join po in _db.ProductionOrders.AsNoTracking() on op.ProductionOrderId equals po.Id
            join so in _db.SalesOrderItems.AsNoTracking() on po.SalesOrderItemId equals so.Id
            join p in _db.Products.AsNoTracking() on so.ProductId equals p.Id
            join pr in _db.PayrollRules.AsNoTracking() on p.PayrollRuleId equals pr.Id into prg
            from pr in prg.DefaultIfEmpty()
            where op.EmployeeId == employee.Id && op.OperationDate == request.Date
            select new
            {
                op.QtySewn,
                op.HoursWorked,
                p.PlanPerHour,
                PremiumPercent = pr != null ? (decimal?)pr.PremiumPercent : null
            }
        ).ToListAsync(cancellationToken);

        if (sewingOps.Count == 0)
            return;

        var hoursWorked = sewingOps.Sum(x => x.HoursWorked);
        if (hoursWorked <= 0)
            return;



        //var ruleByDate =
        //    await _db.PayrollRules
        //        .AsNoTracking()
        //        .Where(x => x.EffectiveFrom <= request.Date)
        //        .OrderByDescending(x => x.EffectiveFrom) // сейчас можно создавать несколько правил в одну дату, поэтому могут быть разночтения и дублирования
        //        .FirstOrDefaultAsync(cancellationToken);

        //var fallbackPremiumPercent = employee.PremiumPercent ?? ruleByDate?.PremiumPercent ?? 0m;

        var qtyProduced = sewingOps.Sum(x => x.QtySewn);
        decimal qtyPlanned = 0m;
        decimal qtyExtra = 0m;
        decimal premiumAmount = 0m;
        //decimal maxPremiumApplied = fallbackPremiumPercent;

        foreach (var op in sewingOps)
        {
            var planPerHour = op.PlanPerHour ?? 0m;
            var planned = planPerHour * op.HoursWorked;
            qtyPlanned += planned;

            var extra = Math.Max(0, op.QtySewn - planned);
            qtyExtra += extra;

            var premiumPercent = employee.PremiumPercent ?? op.PremiumPercent ?? 0m;

            //var premiumPercent = op.ProductPremiumPercent ?? fallbackPremiumPercent;
            //if (premiumPercent > maxPremiumApplied)
            //    maxPremiumApplied = premiumPercent;

            premiumAmount += extra * (employee.RatePerNormHour ?? 0m) * premiumPercent / 100m;
        }

        var baseAmount = hoursWorked * (employee.RatePerNormHour ?? 0m);
        var totalAmount = baseAmount + premiumAmount;

        var existing =
            await _db.PayrollAccruals
                .FirstOrDefaultAsync(x =>
                    x.EmployeeId == employee.Id &&
                    x.AccrualDate == request.Date,
                    cancellationToken);

        if (existing != null)
        {
            _db.PayrollAccruals.Remove(existing);
        }

        _db.PayrollAccruals.Add(new PayrollAccrualEntity(
            employeeId: employee.Id,
            accrualDate: request.Date,
            hoursWorked: hoursWorked,
            qtyPlanned: qtyPlanned,
            qtyProduced: qtyProduced,
            qtyExtra: qtyExtra,
            //premiumPercentApplied: maxPremiumApplied,
            baseAmount,
            premiumAmount,
            totalAmount));

        await _db.SaveChangesAsync(cancellationToken);
    }
}
