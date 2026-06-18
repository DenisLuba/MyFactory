using MyFactory.WebApi.Contracts.Positions;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Positions;

public sealed class PositionDetailsResponseExample : IExamplesProvider<PositionDetailsResponse>
{
    public PositionDetailsResponse GetExamples() => new(
        Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        Name: "Ўве€",
        Code: "SHW",
        DepartmentId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
        DepartmentName: "Ўвейный цех",
        BaseNormPerHour: 2.5m,
        BaseRatePerNormHour: 350m,
        DefaultPremiumPercent: 0.2m,
        CanCut: false,
        CanSew: true,
        CanPackage: false,
        CanHandleMaterials: false,
        IsActive: true);
}
