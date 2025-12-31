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

        var position =
            await _db.Positions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == employee.PositionId, cancellationToken)
            ?? throw new NotFoundException("Position not found");

        var timesheet =
            await _db.Timesheets
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.EmployeeId == employee.Id &&
                    x.WorkDate == request.Date,
                    cancellationToken);

        if (timesheet == null || timesheet.HoursWorked == 0)
            return;

        // TODO: получить фактическое производство
        var qtyProduced = 0m;






        var qtyPlanned =
            (position.BaseNormPerHour ?? 0m) * timesheet.HoursWorked;

        var qtyExtra = Math.Max(0, qtyProduced - qtyPlanned);

        var rule =
            await _db.PayrollRules
                .AsNoTracking()
                .Where(x => x.EffectiveFrom <= request.Date)
                .OrderByDescending(x => x.EffectiveFrom)
                .FirstOrDefaultAsync(cancellationToken);

        var premiumPercent = rule?.PremiumPercent ?? employee.PremiumPercent;

        var baseAmount = timesheet.HoursWorked * employee.RatePerNormHour;
        var premiumAmount = qtyExtra * employee.RatePerNormHour * premiumPercent / 100m;
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
            employee.Id,
            request.Date,
            timesheet.HoursWorked,
            qtyPlanned,
            qtyProduced,
            qtyExtra,
            premiumPercent,
            baseAmount,
            premiumAmount,
            totalAmount));

        await _db.SaveChangesAsync(cancellationToken);
    }
}
