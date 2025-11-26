using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Specifications;

namespace MyFactory.WebApi.SwaggerExamples.Specifications;

public class SpecificationsGetResponseExample : IExamplesProvider<SpecificationsGetResponse>
{
    public SpecificationsGetResponse GetExamples() =>
        new(
            Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            Sku: "SP-001",
            Name: "Пижама женская",
            PlanPerHour: 2.5,
            Bom: new[]
            {
                new BomItemResponse(
                    MaterialId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                    Material: "Ткань Ситец",
                    Qty: 1.8,
                    Unit: "m",
                    Price: 180.0m
                )
            },
            Operations: new[]
            {
                new OperationResponse("CUT", "Раскрой", 6, 15)
            }
        );
}

