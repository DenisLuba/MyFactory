using MediatR;

namespace MyFactory.Application.Features.Positions.CreatePositions;

public sealed record CreatePositionCommand(
    string Name,
    string? Code,
    Guid DepartmentId,

    decimal? BaseNormPerHour,
    decimal? BaseRatePerNormHour,
    decimal? DefaultPremiumPercent,

    bool CanCut,
    bool CanSew,
    bool CanPackage,
    bool CanHandleMaterials
) : IRequest<Guid>;

