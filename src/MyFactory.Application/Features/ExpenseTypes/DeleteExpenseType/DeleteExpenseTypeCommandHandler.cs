using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.ExpenseTypes.DeleteExpenseType;

public sealed class DeleteExpenseTypeCommandHandler : IRequestHandler<DeleteExpenseTypeCommand>
{
    private readonly IApplicationDbContext _db;

    public DeleteExpenseTypeCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(DeleteExpenseTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _db.ExpenseTypes
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("Expense type not found");

        _db.ExpenseTypes.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
