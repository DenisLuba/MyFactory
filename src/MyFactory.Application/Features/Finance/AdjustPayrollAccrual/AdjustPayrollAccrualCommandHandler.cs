using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

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

        accrual.Adjust(request.BaseAmount, request.PremiumAmount, request.Reason);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
