using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.Features.Advances.AddCashAdvanceAmount;

public sealed class AddCashAdvanceAmountCommandHandler : IRequestHandler<AddCashAdvanceAmountCommand>
{
    private readonly IApplicationDbContext _db;

    public AddCashAdvanceAmountCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(AddCashAdvanceAmountCommand request, CancellationToken cancellationToken)
    {
        if (request.Amount <= 0)
            throw new DomainApplicationException("Amount must be greater than zero.");

        var advance = await _db.CashAdvances
            .FirstOrDefaultAsync(x => x.Id == request.CashAdvanceId, cancellationToken)
            ?? throw new NotFoundException("Cash advance not found");

        if (advance.Status == CashAdvanceStatus.Returned)
            throw new DomainApplicationException("Cash advance is closed.");

        advance.Amount += request.Amount;

        await _db.SaveChangesAsync(cancellationToken);
    }
}
