namespace MyFactory.Application.DTOs.Positions;

public sealed class PositionDetailsDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = null!;
    public string? Code { get; init; }

    public Guid DepartmentId { get; init; }
    public string DepartmentName { get; init; } = null!;

    public decimal? BaseNormPerHour { get; init; }
    public decimal? BaseRatePerNormHour { get; init; }
    public decimal? DefaultPremiumPercent { get; init; }

    public bool CanCut { get; init; }
    public bool CanSew { get; init; }
    public bool CanPackage { get; init; }
    public bool CanHandleMaterials { get; init; }

    public bool IsActive { get; init; }
}
