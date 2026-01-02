using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.Expenses.UpdateExpense;

public sealed class UpdateExpenseCommandHandler : IRequestHandler<UpdateExpenseCommand>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public UpdateExpenseCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _db.Expenses
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("Expense not found");

        if (entity.CreatedBy != _currentUser.UserId)
            throw new DomainException("Only the creator can update this expense.");

        var typeExists = await _db.ExpenseTypes
            .AsNoTracking()
            .AnyAsync(x => x.Id == request.ExpenseTypeId, cancellationToken);

        if (!typeExists)
            throw new DomainException("Expense type not found.");

        entity.Update(request.ExpenseTypeId, request.ExpenseDate, request.Amount, request.Description);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
