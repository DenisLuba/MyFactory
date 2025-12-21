using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Advances;

namespace MyFactory.Application.OldFeatures.Advances.Queries.GetAdvances;

public sealed class GetAdvancesQueryHandler : IRequestHandler<GetAdvancesQuery, IReadOnlyCollection<AdvanceDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAdvancesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<AdvanceDto>> Handle(GetAdvancesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Advances
            .AsNoTracking()
            .Include(entity => entity.Employee)
            .Include(entity => entity.Reports)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Status))
        {
            var status = request.Status.Trim();
            query = query.Where(entity => entity.Status == status);
        }

        if (request.EmployeeId.HasValue)
        {
            query = query.Where(entity => entity.EmployeeId == request.EmployeeId);
        }

        var advances = await query
            .OrderBy(entity => entity.IssuedAt)
            .ToListAsync(cancellationToken);

        return advances
            .Select(entity => AdvanceDto.FromEntity(entity, entity.Employee?.FullName ?? string.Empty))
            .ToArray();
    }
}
