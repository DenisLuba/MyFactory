using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.Positions.UpdatePosition;

public sealed class UpdatePositionCommandHandler
    : IRequestHandler<UpdatePositionCommand>
{
    private readonly IApplicationDbContext _db;

    public UpdatePositionCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Handle(
        UpdatePositionCommand request,
        CancellationToken cancellationToken)
    {
        var position =
            await _db.Positions
                .FirstOrDefaultAsync(x => x.Id == request.PositionId, cancellationToken)
            ?? throw new NotFoundException("Position not found");

        var departmentExists = await _db.Departments
            .AnyAsync(x => x.Id == request.DepartmentId, cancellationToken);

        if (!departmentExists)
            throw new NotFoundException("Department not found");

        position.Update(
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

        if (!request.IsActive && position.IsActive)
            position.Deactivate();

        if (request.IsActive && !position.IsActive)
            position.Activate();

        await _db.SaveChangesAsync(cancellationToken);
    }
}
