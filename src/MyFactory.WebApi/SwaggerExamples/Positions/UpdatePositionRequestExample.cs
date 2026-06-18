using MyFactory.WebApi.Contracts.Positions;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Positions;

public sealed class UpdatePositionRequestExample : IExamplesProvider<UpdatePositionRequest>
{
    public UpdatePositionRequest GetExamples() => new(
        Name: "Старший швея",
        Code: "SHW-LEAD",
        DepartmentId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
        BaseNormPerHour: 2.8m,
        BaseRatePerNormHour: 400m,
        DefaultPremiumPercent: 0.25m,
        CanCut: false,
        CanSew: true,
        CanPackage: true,
        CanHandleMaterials: true,
        IsActive: true);
}
