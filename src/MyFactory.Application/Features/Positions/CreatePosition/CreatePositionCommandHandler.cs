using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Domain.Entities.Organization;

namespace MyFactory.Application.Features.Positions.CreatePositions;

public sealed class CreatePositionCommandHandler
    : IRequestHandler<CreatePositionCommand, Guid>
{
    private readonly IApplicationDbContext _db;

    public CreatePositionCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> Handle(
        CreatePositionCommand request,
        CancellationToken cancellationToken)
    {
        var departmentExists = await _db.Departments
            .AnyAsync(x => x.Id == request.DepartmentId, cancellationToken);

        if (!departmentExists)
            throw new NotFoundException("Department not found");

        if (!string.IsNullOrWhiteSpace(request.Code))
        {
            var codeExists = await _db.Positions.AnyAsync(
                x => x.Code == request.Code,
                cancellationToken);

            if (codeExists)
                throw new DomainApplicationException("Position with the same code already exists.");
        }

        var position = new PositionEntity(
            name: request.Name,
            departmentId: request.DepartmentId,
            code: request.Code,
            baseNormPerHour: request.BaseNormPerHour,
            baseRatePerNormHour: request.BaseRatePerNormHour,
            defaultPremiumPercent: request.DefaultPremiumPercent,
            canCut: request.CanCut,
            canSew: request.CanSew,
            canPackage: request.CanPackage,
            canHandleMaterials: request.CanHandleMaterials
        );

        _db.Positions.Add(position);
        await _db.SaveChangesAsync(cancellationToken);

        return position.Id;
    }
}
