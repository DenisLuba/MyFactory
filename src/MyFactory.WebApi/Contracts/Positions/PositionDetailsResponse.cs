namespace MyFactory.WebApi.Contracts.Positions;

public record PositionDetailsResponse(
    Guid Id,
    string Name,
    string? Code,
    Guid DepartmentId,
    string DepartmentName,
    decimal? BaseNormPerHour,
    decimal? BaseRatePerNormHour,
    decimal? DefaultPremiumPercent,
    bool CanCut,
    bool CanSew,
    bool CanPackage,
    bool CanHandleMaterials,
    bool IsActive);
