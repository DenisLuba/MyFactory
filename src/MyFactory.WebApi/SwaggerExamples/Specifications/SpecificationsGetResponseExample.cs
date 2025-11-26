using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Specifications;
using MyFactory.WebApi.Contracts.Materials;

namespace MyFactory.WebApi.SwaggerExamples.Specifications;

public class SpecificationsGetResponseExample : IExamplesProvider<SpecificationsGetResponse>
{
    public SpecificationsGetResponse GetExamples() =>
        new(
            Id: Guid.Parse("11111111-1111-1111-1111-111111111000"),
                Sku: "SP-001",
                Name: "Пижама женская",
                PlanPerHour: 2.5,
                Bom:
                [
                    new BomItemResponse(
                        MaterialId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        MaterialName: "Ткань Ситец",
                        Quantity: 1.8,
                        Unit: Units.Meter                        ,
                        Price: 180m
                    )
                ],
                Operations:
                [
                    new OperationItemResponse(
                        OperationId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        Code: "CUT",
                        Name: "Раскрой",
                        Minutes: 6,
                        Cost: 15
                    )
                ]
        );
}

