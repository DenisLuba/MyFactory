using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Specifications;

namespace MyFactory.Application.OldFeatures.Specifications.Commands.AddOperation;

public sealed class AddSpecificationOperationCommandHandler : IRequestHandler<AddSpecificationOperationCommand, SpecificationOperationResultDto>
{
    private readonly IApplicationDbContext _context;

    public AddSpecificationOperationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SpecificationOperationResultDto> Handle(AddSpecificationOperationCommand request, CancellationToken cancellationToken)
    {
        var specification = await _context.Specifications
            .Include(spec => spec.Operations)
            .FirstOrDefaultAsync(spec => spec.Id == request.SpecificationId, cancellationToken);

        if (specification is null)
        {
            throw new InvalidOperationException("Specification not found.");
        }

        var operation = await _context.Operations
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == request.OperationId, cancellationToken)
            ?? throw new InvalidOperationException("Operation not found.");

        var workshop = await _context.Workshops
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == request.WorkshopId, cancellationToken)
            ?? throw new InvalidOperationException("Workshop not found.");

        if (!workshop.IsActive)
        {
            throw new InvalidOperationException("Workshop is inactive.");
        }

        var operationItem = specification.AddOperation(
            request.OperationId,
            request.WorkshopId,
            request.TimeMinutes,
            request.OperationCost);

        await _context.SpecificationOperations.AddAsync(operationItem, cancellationToken);
        specification.ChangeStatus(SpecificationsStatusValues.OperationAdded);
        await _context.SaveChangesAsync(cancellationToken);

        var dto = SpecificationOperationItemDto.FromEntity(operationItem, operation.Name, workshop.Name);
        return new SpecificationOperationResultDto(specification.Id, dto, SpecificationsStatusValues.OperationAdded);
    }
}
