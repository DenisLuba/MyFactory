using System;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Specifications;

namespace MyFactory.WebApi.SwaggerExamples.Specifications;

public class SpecificationsAddBomResponseExample : IExamplesProvider<SpecificationsAddBomResponse>
{
    public SpecificationsAddBomResponse GetExamples() =>
        new(
            SpecificationId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            Item: new SpecificationBomItemResponse(
                Id: Guid.NewGuid(),
                Material: "Ткань Ситец",
                Quantity: 1.8,
                Unit: "м",
                Price: 180,
                Cost: 324
            ),
            Status: SpecificationsStatus.BomAdded
        );
}
