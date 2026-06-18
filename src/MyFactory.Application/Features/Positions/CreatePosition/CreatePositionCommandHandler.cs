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

        // Try to reuse existing position by code or name
        PositionEntity? existing = null;
        if (!string.IsNullOrWhiteSpace(request.Code))
        {
            existing = await _db.Positions
                .Include(p => p.DepartmentPositions)
                .FirstOrDefaultAsync(x => x.Code == request.Code, cancellationToken);
        }

        if (existing is null)
        {
            var nameNormalized = request.Name.Trim();
            existing = await _db.Positions
                .Include(p => p.DepartmentPositions)
                .FirstOrDefaultAsync(x => x.Name.ToLower() == nameNormalized.ToLower(), cancellationToken);
        }

        if (existing is not null)
        {
            var alreadyLinked = existing.DepartmentPositions.Any(dp => dp.DepartmentId == request.DepartmentId);
            if (alreadyLinked)
                throw new DomainApplicationException("Position already exists in this department.");

            existing.AddDepartment(request.DepartmentId);
            await _db.SaveChangesAsync(cancellationToken);
            return existing.Id;
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
