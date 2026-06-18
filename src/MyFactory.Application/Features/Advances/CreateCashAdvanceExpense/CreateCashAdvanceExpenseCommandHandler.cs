using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Finance;

namespace MyFactory.Application.Features.Advances.CreateCashAdvanceExpense;

public sealed class CreateCashAdvanceExpenseCommandHandler : IRequestHandler<CreateCashAdvanceExpenseCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public CreateCashAdvanceExpenseCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(CreateCashAdvanceExpenseCommand request, CancellationToken cancellationToken)
    {
        var advance = await _db.CashAdvances
            .FirstOrDefaultAsync(x => x.Id == request.CashAdvanceId, cancellationToken)
            ?? throw new NotFoundException("Cash advance not found");

        if (advance.Status == CashAdvanceStatus.Returned)
            throw new DomainApplicationException("Cash advance is closed.");

        var entity = new CashAdvanceExpenseEntity(
            request.CashAdvanceId,
            request.ExpenseDate,
            request.Amount,
            request.Description ?? string.Empty);

        _db.CashAdvanceExpenses.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
