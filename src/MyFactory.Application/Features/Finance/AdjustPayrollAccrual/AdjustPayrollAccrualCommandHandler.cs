using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.Features.Finance.AdjustPayrollAccrual;

public sealed class AdjustPayrollAccrualCommandHandler
    : IRequestHandler<AdjustPayrollAccrualCommand>
{
    private readonly IApplicationDbContext _db;

    public AdjustPayrollAccrualCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        AdjustPayrollAccrualCommand request,
        CancellationToken cancellationToken)
    {
        var accrual =
            await _db.PayrollAccruals
                .FirstOrDefaultAsync(x => x.Id == request.AccrualId, cancellationToken)
            ?? throw new NotFoundException("Payroll accrual not found");

        var total = request.BaseAmount + request.PremiumAmount;

        _db.PayrollAccruals.Remove(accrual);

        _db.PayrollAccruals.Add(new PayrollAccrualEntity(
            accrual.EmployeeId,
            accrual.AccrualDate,
            accrual.HoursWorked,
            accrual.QtyPlanned,
            accrual.QtyProduced,
            accrual.QtyExtra,
            accrual.PremiumPercentApplied,
            request.BaseAmount,
            request.PremiumAmount,
            total));

        await _db.SaveChangesAsync(cancellationToken);
    }
}
