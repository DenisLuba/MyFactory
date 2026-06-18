using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.Advances.CloseCashAdvance;

public sealed class CloseCashAdvanceCommandHandler : IRequestHandler<CloseCashAdvanceCommand>
{
    private readonly IApplicationDbContext _db;

    public CloseCashAdvanceCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(CloseCashAdvanceCommand request, CancellationToken cancellationToken)
    {
        var advance = await _db.CashAdvances
            .FirstOrDefaultAsync(x => x.Id == request.CashAdvanceId, cancellationToken)
            ?? throw new NotFoundException("Cash advance not found");

        var spent = await _db.CashAdvanceExpenses
            .AsNoTracking()
            .Where(x => x.CashAdvanceId == advance.Id)
            .SumAsync(x => (decimal?)x.Amount, cancellationToken) ?? 0m;

        var returned = await _db.CashAdvanceReturns
            .AsNoTracking()
            .Where(x => x.CashAdvanceId == advance.Id)
            .SumAsync(x => (decimal?)x.Amount, cancellationToken) ?? 0m;

        var balance = advance.Amount - spent - returned;

        advance.Close(balance);

        await _db.SaveChangesAsync(cancellationToken);
    }
}
