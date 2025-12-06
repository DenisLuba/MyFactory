using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Specifications;

namespace MyFactory.Application.Features.Specifications.Queries.GetSpecification;

public sealed class GetSpecificationQueryHandler : IRequestHandler<GetSpecificationQuery, SpecificationDetailsDto>
{
    private readonly IApplicationDbContext _context;

    public GetSpecificationQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SpecificationDetailsDto> Handle(GetSpecificationQuery request, CancellationToken cancellationToken)
    {
        var specification = await _context.Specifications
            .AsNoTracking()
            .FirstOrDefaultAsync(spec => spec.Id == request.SpecificationId, cancellationToken)
            ?? throw new InvalidOperationException("Specification not found.");

        return SpecificationDetailsDto.FromEntity(specification);
    }
}
