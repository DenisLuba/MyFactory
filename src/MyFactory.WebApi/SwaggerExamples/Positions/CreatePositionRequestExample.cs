using MyFactory.WebApi.Contracts.Positions;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Positions;

public sealed class CreatePositionRequestExample : IExamplesProvider<CreatePositionRequest>
{
    public CreatePositionRequest GetExamples() => new(
        Name: "Швея",
        Code: "SHW",
        DepartmentId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
        BaseNormPerHour: 2.5m,
        BaseRatePerNormHour: 350m,
        DefaultPremiumPercent: 0.2m,
        CanCut: false,
        CanSew: true,
        CanPackage: false,
        CanHandleMaterials: true);
}
