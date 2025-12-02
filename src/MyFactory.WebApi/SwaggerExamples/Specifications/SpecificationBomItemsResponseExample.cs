using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Specifications;

namespace MyFactory.WebApi.SwaggerExamples.Specifications;

public class SpecificationBomItemsResponseExample : IExamplesProvider<IEnumerable<SpecificationBomItemResponse>>
{
    public IEnumerable<SpecificationBomItemResponse> GetExamples() => new[]
    {
        new SpecificationBomItemResponse(
            Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
            Material: "Ткань Ситец",
            Quantity: 1.8,
            Unit: "м",
            Price: 180,
            Cost: 324
        ),
        new SpecificationBomItemResponse(
            Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
            Material: "Фурнитура",
            Quantity: 1,
            Unit: "комплект",
            Price: 60,
            Cost: 60
        )
    };
}
