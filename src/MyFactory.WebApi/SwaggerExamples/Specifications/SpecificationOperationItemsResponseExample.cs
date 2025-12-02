using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Specifications;

namespace MyFactory.WebApi.SwaggerExamples.Specifications;

public class SpecificationOperationItemsResponseExample : IExamplesProvider<IEnumerable<SpecificationOperationItemResponse>>
{
    public IEnumerable<SpecificationOperationItemResponse> GetExamples() => new[]
    {
        new SpecificationOperationItemResponse(
            Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
            Operation: "Раскрой",
            Minutes: 8,
            Cost: 24
        ),
        new SpecificationOperationItemResponse(
            Id: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
            Operation: "Сборка",
            Minutes: 12,
            Cost: 44
        )
    };
}
