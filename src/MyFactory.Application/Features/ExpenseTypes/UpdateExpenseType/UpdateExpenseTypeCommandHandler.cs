using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.ExpenseTypes.UpdateExpenseType;

public sealed class UpdateExpenseTypeCommandHandler : IRequestHandler<UpdateExpenseTypeCommand>
{
    private readonly IApplicationDbContext _db;

    public UpdateExpenseTypeCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(UpdateExpenseTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _db.ExpenseTypes
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("Expense type not found");

        var nameTaken = await _db.ExpenseTypes
            .AsNoTracking()
            .AnyAsync(x => x.Id != request.Id && x.Name == request.Name, cancellationToken);

        if (nameTaken)
            throw new DomainException("Expense type with the same name already exists.");

        entity.Update(request.Name, request.Description);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
