using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Positions;

namespace MyFactory.Application.Features.Positions.GetPositionDetails;

public sealed class GetPositionDetailsQueryHandler
    : IRequestHandler<GetPositionDetailsQuery, PositionDetailsDto>
{
    private readonly IApplicationDbContext _db;

    public GetPositionDetailsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<PositionDetailsDto> Handle(
        GetPositionDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var position =
            await _db.Positions
                .AsNoTracking()
                .Include(p => p.DepartmentPositions)
                .FirstOrDefaultAsync(x => x.Id == request.PositionId, cancellationToken)
            ?? throw new NotFoundException("Position not found");

        var departmentId = position.DepartmentPositions.FirstOrDefault()?.DepartmentId;

        // Fallback for legacy data without department link
        var department = departmentId.HasValue
            ? await _db.Departments
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == departmentId.Value, cancellationToken)
            : null;

        if (department is null)
        {
            department = await _db.Departments
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .FirstOrDefaultAsync(cancellationToken)
                ?? throw new NotFoundException("Department not found");
        }

        departmentId = department.Id;

        return new PositionDetailsDto
        {
            Id = position.Id,
            Name = position.Name,
            Code = position.Code,
            DepartmentId = department.Id,
            DepartmentName = department.Name,

            BaseNormPerHour = position.BaseNormPerHour,
            BaseRatePerNormHour = position.BaseRatePerNormHour,
            DefaultPremiumPercent = position.DefaultPremiumPercent,

            CanCut = position.CanCut,
            CanSew = position.CanSew,
            CanPackage = position.CanPackage,
            CanHandleMaterials = position.CanHandleMaterials,

            IsActive = position.IsActive
        };
    }
}
