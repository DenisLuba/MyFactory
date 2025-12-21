using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Finance;

namespace MyFactory.Application.OldFeatures.Finance.Queries.GetExpenseTypes;

public sealed class GetExpenseTypesQueryHandler : IRequestHandler<GetExpenseTypesQuery, IReadOnlyCollection<ExpenseTypeDto>>
{
    private readonly IApplicationDbContext _context;

    public GetExpenseTypesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<ExpenseTypeDto>> Handle(GetExpenseTypesQuery request, CancellationToken cancellationToken)
    {
        var expenseTypes = await _context.ExpenseTypes
            .AsNoTracking()
            .OrderBy(entity => entity.Name)
            .ThenBy(entity => entity.Category)
            .ToListAsync(cancellationToken);

        return expenseTypes
            .Select(ExpenseTypeDto.FromEntity)
            .ToList();
    }
}
