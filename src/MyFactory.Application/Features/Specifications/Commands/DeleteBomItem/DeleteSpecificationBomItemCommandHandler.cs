using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Specifications;

namespace MyFactory.Application.Features.Specifications.Commands.DeleteBomItem;

public sealed class DeleteSpecificationBomItemCommandHandler : IRequestHandler<DeleteSpecificationBomItemCommand, SpecificationDeleteBomItemResultDto>
{
    private readonly IApplicationDbContext _context;

    public DeleteSpecificationBomItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SpecificationDeleteBomItemResultDto> Handle(DeleteSpecificationBomItemCommand request, CancellationToken cancellationToken)
    {
        var bomItem = await _context.SpecificationBomItems
            .Include(item => item.Specification)
            .FirstOrDefaultAsync(item =>
                item.Id == request.BomItemId && item.SpecificationId == request.SpecificationId,
                cancellationToken);

        if (bomItem is null)
        {
            throw new InvalidOperationException("BOM item not found.");
        }

        var specification = bomItem.Specification;
        if (specification is null)
        {
            specification = await _context.Specifications
                .FirstOrDefaultAsync(spec => spec.Id == request.SpecificationId, cancellationToken)
                ?? throw new InvalidOperationException("Specification not found.");
        }

        _context.SpecificationBomItems.Remove(bomItem);
        specification.ChangeStatus(SpecificationsStatusValues.BomDeleted);
        await _context.SaveChangesAsync(cancellationToken);

        return new SpecificationDeleteBomItemResultDto(specification.Id, bomItem.Id, SpecificationsStatusValues.BomDeleted);
    }
}
