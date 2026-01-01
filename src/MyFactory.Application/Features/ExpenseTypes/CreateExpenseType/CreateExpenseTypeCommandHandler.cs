using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Finance;
using MyFactory.Domain.Exceptions;

namespace MyFactory.Application.Features.ExpenseTypes.CreateExpenseType;

public sealed class CreateExpenseTypeCommandHandler
    : IRequestHandler<CreateExpenseTypeCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public CreateExpenseTypeCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(CreateExpenseTypeCommand request, CancellationToken cancellationToken)
    {
        var exists = await _db.ExpenseTypes
            .AsNoTracking()
            .AnyAsync(x => x.Name == request.Name, cancellationToken);

        if (exists)
            throw new DomainException("Expense type with the same name already exists.");

        var entity = new ExpenseTypeEntity(request.Name, request.Description);
        _db.ExpenseTypes.Add(entity);

        await _db.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
