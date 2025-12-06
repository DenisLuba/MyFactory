using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Specifications;

namespace MyFactory.Application.Features.Specifications.Commands.AddBomItem;

public sealed class AddSpecificationBomItemCommandHandler : IRequestHandler<AddSpecificationBomItemCommand, SpecificationBomItemResultDto>
{
    private readonly IApplicationDbContext _context;

    public AddSpecificationBomItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SpecificationBomItemResultDto> Handle(AddSpecificationBomItemCommand request, CancellationToken cancellationToken)
    {
        var specification = await _context.Specifications
            .Include(spec => spec.BomItems)
            .FirstOrDefaultAsync(spec => spec.Id == request.SpecificationId, cancellationToken);

        if (specification is null)
        {
            throw new InvalidOperationException("Specification not found.");
        }

        var material = await _context.Materials
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == request.MaterialId, cancellationToken);

        if (material is null)
        {
            throw new InvalidOperationException("Material not found.");
        }

        var bomItem = specification.AddBomItem(
            request.MaterialId,
            request.Quantity,
            request.Unit.Trim(),
            request.UnitCost);

        await _context.SpecificationBomItems.AddAsync(bomItem, cancellationToken);
        specification.ChangeStatus(SpecificationsStatusValues.BomAdded);
        await _context.SaveChangesAsync(cancellationToken);

        var dto = SpecificationBomItemDto.FromEntity(bomItem, material.Name);
        return new SpecificationBomItemResultDto(specification.Id, dto, SpecificationsStatusValues.BomAdded);
    }
}
