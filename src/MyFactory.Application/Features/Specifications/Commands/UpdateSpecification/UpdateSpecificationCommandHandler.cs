using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Specifications;

namespace MyFactory.Application.Features.Specifications.Commands.UpdateSpecification;

public sealed class UpdateSpecificationCommandHandler : IRequestHandler<UpdateSpecificationCommand, SpecificationMutationResultDto>
{
    private readonly IApplicationDbContext _context;

    public UpdateSpecificationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SpecificationMutationResultDto> Handle(UpdateSpecificationCommand request, CancellationToken cancellationToken)
    {
        var specification = await _context.Specifications
            .FirstOrDefaultAsync(spec => spec.Id == request.SpecificationId, cancellationToken);

        if (specification is null)
        {
            throw new InvalidOperationException("Specification not found.");
        }

        var sku = request.Sku.Trim();
        var duplicateExists = await _context.Specifications
            .AsNoTracking()
            .AnyAsync(spec => spec.Id != request.SpecificationId && spec.Sku == sku, cancellationToken);

        if (duplicateExists)
        {
            throw new InvalidOperationException("Another specification with the same SKU already exists.");
        }

        specification.UpdateSku(sku);
        specification.Rename(request.Name.Trim());
        specification.UpdatePlanPerHour(request.PlanPerHour);
        specification.UpdateDescription(request.Description?.Trim());
        specification.ChangeStatus(SpecificationsStatusValues.Updated);

        await _context.SaveChangesAsync(cancellationToken);

        return new SpecificationMutationResultDto(specification.Id, SpecificationsStatusValues.Updated);
    }
}
