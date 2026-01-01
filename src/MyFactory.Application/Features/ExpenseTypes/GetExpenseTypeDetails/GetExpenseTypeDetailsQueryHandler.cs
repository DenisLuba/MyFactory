using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.ExpenseTypes;

namespace MyFactory.Application.Features.ExpenseTypes.GetExpenseTypeDetails;

public sealed class GetExpenseTypeDetailsQueryHandler
    : IRequestHandler<GetExpenseTypeDetailsQuery, ExpenseTypeDto>
{
    private readonly IApplicationDbContext _db;

    public GetExpenseTypeDetailsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<ExpenseTypeDto> Handle(GetExpenseTypeDetailsQuery request, CancellationToken cancellationToken)
    {
        var entity = await _db.ExpenseTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new NotFoundException("Expense type not found");

        return new ExpenseTypeDto(entity.Id, entity.Name, entity.Description);
    }
}
