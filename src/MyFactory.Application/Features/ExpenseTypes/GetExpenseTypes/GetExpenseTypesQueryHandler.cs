using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.ExpenseTypes;

namespace MyFactory.Application.Features.ExpenseTypes.GetExpenseTypes;

public sealed class GetExpenseTypesQueryHandler
    : IRequestHandler<GetExpenseTypesQuery, IReadOnlyList<ExpenseTypeDto>>
{
    private readonly IApplicationDbContext _db;

    public GetExpenseTypesQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<ExpenseTypeDto>> Handle(GetExpenseTypesQuery request, CancellationToken cancellationToken)
    {
        return await _db.ExpenseTypes
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new ExpenseTypeDto(x.Id, x.Name, x.Description))
            .ToListAsync(cancellationToken);
    }
}
