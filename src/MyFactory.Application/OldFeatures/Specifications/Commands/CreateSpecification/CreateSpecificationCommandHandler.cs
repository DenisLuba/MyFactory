using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Specifications;
using MyFactory.Domain.Entities.Specifications;

namespace MyFactory.Application.OldFeatures.Specifications.Commands.CreateSpecification;

public sealed class CreateSpecificationCommandHandler : IRequestHandler<CreateSpecificationCommand, SpecificationMutationResultDto>
{
    private readonly IApplicationDbContext _context;

    public CreateSpecificationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SpecificationMutationResultDto> Handle(CreateSpecificationCommand request, CancellationToken cancellationToken)
    {
        var sku = request.Sku.Trim();
        var exists = await _context.Specifications
            .AsNoTracking()
            .AnyAsync(spec => spec.Sku == sku, cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException("Specification with the same SKU already exists.");
        }

        var specification = new Specification(
            sku,
            request.Name.Trim(),
            request.PlanPerHour,
            SpecificationsStatusValues.Created,
            DateTime.UtcNow,
            request.Description?.Trim());

        await _context.Specifications.AddAsync(specification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new SpecificationMutationResultDto(specification.Id, SpecificationsStatusValues.Created);
    }
}
