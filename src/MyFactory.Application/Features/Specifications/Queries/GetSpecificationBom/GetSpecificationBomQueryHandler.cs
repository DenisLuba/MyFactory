using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Specifications;

namespace MyFactory.Application.Features.Specifications.Queries.GetSpecificationBom;

public sealed class GetSpecificationBomQueryHandler : IRequestHandler<GetSpecificationBomQuery, IReadOnlyCollection<SpecificationBomItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetSpecificationBomQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<SpecificationBomItemDto>> Handle(GetSpecificationBomQuery request, CancellationToken cancellationToken)
    {
        var items = await _context.SpecificationBomItems
            .AsNoTracking()
            .Where(item => item.SpecificationId == request.SpecificationId)
            .Include(item => item.Material)
            .OrderBy(item => item.Material!.Name)
            .ToListAsync(cancellationToken);

        return items
            .Select(item => SpecificationBomItemDto.FromEntity(item, item.Material?.Name ?? string.Empty))
            .ToList();
    }
}
