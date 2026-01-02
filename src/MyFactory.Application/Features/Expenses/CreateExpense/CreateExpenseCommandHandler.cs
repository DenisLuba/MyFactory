using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Finance;
using MyFactory.Domain.Exceptions;

namespace MyFactory.Application.Features.Expenses.CreateExpense;

public sealed class CreateExpenseCommandHandler : IRequestHandler<CreateExpenseCommand, Guid>
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUserService _currentUser;

    public CreateExpenseCommandHandler(IApplicationDbContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var typeExists = await _db.ExpenseTypes
            .AsNoTracking()
            .AnyAsync(x => x.Id == request.ExpenseTypeId, cancellationToken);

        if (!typeExists)
            throw new DomainException("Expense type not found.");

        var createdBy = _currentUser.UserId;
        if (createdBy == Guid.Empty)
            throw new DomainException("User is not authenticated.");

        var entity = new ExpenseEntity(request.ExpenseTypeId, request.ExpenseDate, request.Amount, request.Description, createdBy);
        _db.Expenses.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
