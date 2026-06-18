using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.Features.Finance.CalculateDailyPayrollAccrual;

namespace MyFactory.Application.Features.Finance.CalculateDailyPayrollAccrualsForPeriod;

public sealed class CalculatePayrollAccrualsForPeriodCommandHandler
    : IRequestHandler<CalculatePayrollAccrualsForPeriodCommand>
{
    private readonly IMediator _mediator;
    private readonly IApplicationDbContext _db;

    public CalculatePayrollAccrualsForPeriodCommandHandler(
        IMediator mediator,
        IApplicationDbContext db)
    {
        _mediator = mediator;
        _db = db;
    }

    public async Task Handle(
        CalculatePayrollAccrualsForPeriodCommand request,
        CancellationToken cancellationToken)
    {
        var from = new DateOnly(request.Period.Year, request.Period.Month, 1);
        var to = from.AddMonths(1).AddDays(-1);

        var employees =
            await _db.Employees
                .AsNoTracking()
                .Where(x => x.IsActive)
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);

        foreach (var employeeId in employees)
        {
            for (var date = from; date <= to; date = date.AddDays(1))
            {
                await _mediator.Send(
                    new CalculateDailyPayrollAccrualCommand(employeeId, date),
                    cancellationToken);
            }
        }
    }
}
