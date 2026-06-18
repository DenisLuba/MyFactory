namespace MyFactory.MauiClient.Models.Positions;

public record CreatePositionRequest(
    string Name,
    string? Code,
    Guid DepartmentId,
    decimal? BaseNormPerHour,
    decimal? BaseRatePerNormHour,
    decimal? DefaultPremiumPercent,
    bool CanCut,
    bool CanSew,
    bool CanPackage,
    bool CanHandleMaterials);
