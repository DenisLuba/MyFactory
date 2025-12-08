using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Finance;

namespace MyFactory.Application.Features.Finance.Queries.GetOverheadByPeriod;

public sealed class GetOverheadByPeriodQueryHandler : IRequestHandler<GetOverheadByPeriodQuery, IReadOnlyCollection<OverheadMonthlyDto>>
{
    private readonly IApplicationDbContext _context;

    public GetOverheadByPeriodQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<OverheadMonthlyDto>> Handle(GetOverheadByPeriodQuery request, CancellationToken cancellationToken)
    {
        var overheadEntries = await _context.OverheadMonthlyEntries
            .AsNoTracking()
            .Include(entity => entity.ExpenseType)
            .Where(entity => entity.PeriodMonth == request.PeriodMonth && entity.PeriodYear == request.PeriodYear)
            .OrderBy(entity => entity.ExpenseType!.Name)
            .ToListAsync(cancellationToken);

        return overheadEntries
            .Select(entity => OverheadMonthlyDto.FromEntity(entity, entity.ExpenseType?.Name ?? string.Empty))
            .ToList();
    }
}
