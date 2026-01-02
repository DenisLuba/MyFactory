using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Expenses;

namespace MyFactory.Application.Features.Expenses.GetExpenses;

public sealed class GetExpensesQueryHandler
    : IRequestHandler<GetExpensesQuery, IReadOnlyList<ExpenseListItemDto>>
{
    private readonly IApplicationDbContext _db;

    public GetExpensesQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<ExpenseListItemDto>> Handle(GetExpensesQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Expenses
            .AsNoTracking()
            .Where(x => x.ExpenseDate >= request.From && x.ExpenseDate <= request.To);

        if (request.ExpenseTypeId is Guid typeId)
        {
            query = query.Where(x => x.ExpenseTypeId == typeId);
        }

        return await query
            .Join(_db.ExpenseTypes.AsNoTracking(), e => e.ExpenseTypeId, t => t.Id, (e, t) => new { e, t })
            .OrderByDescending(x => x.e.ExpenseDate)
            .ThenBy(x => x.t.Name)
            .Select(x => new ExpenseListItemDto(
                x.e.Id,
                x.e.ExpenseDate,
                x.t.Name,
                x.e.Amount,
                x.e.Description,
                x.e.CreatedBy.ToString()))
            .ToListAsync(cancellationToken);
    }
}
