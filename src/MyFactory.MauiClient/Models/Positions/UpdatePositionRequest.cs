namespace MyFactory.MauiClient.Models.Positions;

public record UpdatePositionRequest(
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
    bool IsActive);
