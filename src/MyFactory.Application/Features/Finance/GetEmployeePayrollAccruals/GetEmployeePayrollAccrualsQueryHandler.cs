using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Finance;

namespace MyFactory.Application.Features.Finance.GetEmployeePayrollAccruals;

public sealed class GetEmployeePayrollAccrualsQueryHandler
    : IRequestHandler<GetEmployeePayrollAccrualsQuery, EmployeePayrollAccrualDetailsDto>
{
    private readonly IApplicationDbContext _db;

    public GetEmployeePayrollAccrualsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<EmployeePayrollAccrualDetailsDto> Handle(
        GetEmployeePayrollAccrualsQuery request,
        CancellationToken cancellationToken)
    {
        var employee =
            await (
                from e in _db.Employees.AsNoTracking()
                join p in _db.Positions.AsNoTracking()
                    on e.PositionId equals p.Id
                where e.Id == request.EmployeeId
                select new
                {
                    e.Id,
                    e.FullName,
                    PositionName = p.Name
                }
            ).FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("Employee not found");

        var from = new DateOnly(request.Period.Year, request.Period.Month, 1);
        var to = from.AddMonths(1).AddDays(-1);

        var accruals =
            await _db.PayrollAccruals
                .AsNoTracking()
                .Where(x =>
                    x.EmployeeId == request.EmployeeId &&
                    x.AccrualDate >= from &&
                    x.AccrualDate <= to)
                .OrderBy(x => x.AccrualDate)
                .ToListAsync(cancellationToken);

        var paidAmount =
            await _db.PayrollPayments
                .AsNoTracking()
                .Where(x =>
                    x.EmployeeId == request.EmployeeId &&
                    x.PaymentDate >= from &&
                    x.PaymentDate <= to)
                .SumAsync(x => x.Amount, cancellationToken);

        return new EmployeePayrollAccrualDetailsDto
        {
            EmployeeId = employee.Id,
            EmployeeName = employee.FullName,
            PositionName = employee.PositionName,
            Period = request.Period,

            TotalBaseAmount = accruals.Sum(x => x.BaseAmount),
            TotalPremiumAmount = accruals.Sum(x => x.PremiumAmount),
            TotalAmount = accruals.Sum(x => x.TotalAmount),

            PaidAmount = paidAmount,
            RemainingAmount = accruals.Sum(x => x.TotalAmount) - paidAmount,

            Days = accruals.Select(x => new EmployeePayrollAccrualDailyDto
            {
                AccrualId = x.Id,
                Date = x.AccrualDate,
                HoursWorked = x.HoursWorked,
                QtyPlanned = x.QtyPlanned,
                QtyProduced = x.QtyProduced,
                QtyExtra = x.QtyExtra,
                BaseAmount = x.BaseAmount,
                PremiumAmount = x.PremiumAmount,
                TotalAmount = x.TotalAmount
            }).ToList()
        };
    }
}
