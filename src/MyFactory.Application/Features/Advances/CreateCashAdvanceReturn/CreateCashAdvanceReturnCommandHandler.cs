using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.Features.Advances.CreateCashAdvanceReturn;

public sealed class CreateCashAdvanceReturnCommandHandler : IRequestHandler<CreateCashAdvanceReturnCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public CreateCashAdvanceReturnCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(CreateCashAdvanceReturnCommand request, CancellationToken cancellationToken)
    {
        var advance = await _db.CashAdvances
            .FirstOrDefaultAsync(x => x.Id == request.CashAdvanceId, cancellationToken)
            ?? throw new NotFoundException("Cash advance not found");

        if (advance.Status == CashAdvanceStatus.Returned)
            throw new DomainException("Cash advance is closed.");

        var entity = new CashAdvanceReturnEntity(
            request.CashAdvanceId,
            request.ReturnDate,
            request.Amount,
            request.Description ?? string.Empty);

        _db.CashAdvanceReturns.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
