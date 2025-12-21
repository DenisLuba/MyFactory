using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Specifications;

namespace MyFactory.Application.OldFeatures.Specifications.Queries.GetSpecificationOperations;

public sealed class GetSpecificationOperationsQueryHandler : IRequestHandler<GetSpecificationOperationsQuery, IReadOnlyCollection<SpecificationOperationItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetSpecificationOperationsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<SpecificationOperationItemDto>> Handle(GetSpecificationOperationsQuery request, CancellationToken cancellationToken)
    {
        var operations = await _context.SpecificationOperations
            .AsNoTracking()
            .Where(item => item.SpecificationId == request.SpecificationId)
            .Include(item => item.Operation)
            .Include(item => item.Workshop)
            .OrderBy(item => item.Operation!.Name)
            .ToListAsync(cancellationToken);

        return operations
            .Select(item => SpecificationOperationItemDto.FromEntity(
                item,
                item.Operation?.Name ?? string.Empty,
                item.Workshop?.Name ?? string.Empty))
            .ToList();
    }
}
