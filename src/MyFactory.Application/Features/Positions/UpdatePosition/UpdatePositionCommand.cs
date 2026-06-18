using MediatR;

namespace MyFactory.Application.Features.Positions.UpdatePosition;

public sealed record UpdatePositionCommand(
    Guid PositionId,

    string Name,
    string? Code,
    Guid DepartmentId,

    decimal? BaseNormPerHour,
    decimal? BaseRatePerNormHour,
    decimal? DefaultPremiumPercent,

    bool CanCut,
    bool CanSew,
    bool CanPackage,
    bool CanHandleMaterials,

    bool IsActive
) : IRequest;
