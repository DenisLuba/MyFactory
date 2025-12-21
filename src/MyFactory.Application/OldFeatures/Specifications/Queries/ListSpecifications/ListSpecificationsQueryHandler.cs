using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Specifications;

namespace MyFactory.Application.OldFeatures.Specifications.Queries.ListSpecifications;

public sealed class ListSpecificationsQueryHandler : IRequestHandler<ListSpecificationsQuery, IReadOnlyCollection<SpecificationListItemDto>>
{
    private readonly IApplicationDbContext _context;

    public ListSpecificationsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<SpecificationListItemDto>> Handle(ListSpecificationsQuery request, CancellationToken cancellationToken)
    {
        var specifications = await _context.Specifications
            .AsNoTracking()
            .OrderBy(spec => spec.Sku)
            .ToListAsync(cancellationToken);

        return specifications
            .Select(spec => SpecificationListItemDto.FromEntity(spec))
            .ToList();
    }
}
