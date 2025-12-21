using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.FinishedGoods;
using MyFactory.Application.Features.FinishedGoods.Common;
using MyFactory.Domain.Enums;

namespace MyFactory.Application.OldFeatures.FinishedGoods.Queries.GetReturns;

public sealed class GetReturnsQueryHandler : IRequestHandler<GetReturnsQuery, IReadOnlyCollection<ReturnDto>>
{
    private readonly IApplicationDbContext _context;

    public GetReturnsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<ReturnDto>> Handle(GetReturnsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.CustomerReturns
            .AsNoTracking()
            .Include(entity => entity.Items)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Status) && Enum.TryParse<ReturnStatus>(request.Status.Trim(), true, out var parsedStatus))
        {
            query = query.Where(entity => entity.Status == parsedStatus);
        }

        var returns = await query
            .OrderByDescending(entity => entity.ReturnDate)
            .ThenBy(entity => entity.ReturnNumber)
            .ToListAsync(cancellationToken);

        return await ReturnDtoFactory.CreateAsync(_context, returns, cancellationToken);
    }
}
