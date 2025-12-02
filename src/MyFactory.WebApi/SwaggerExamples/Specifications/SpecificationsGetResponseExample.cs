using System;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Specifications;

namespace MyFactory.WebApi.SwaggerExamples.Specifications;

public class SpecificationsGetResponseExample : IExamplesProvider<SpecificationsGetResponse>
{
    public SpecificationsGetResponse GetExamples() =>
        new(
            Id: Guid.Parse("11111111-1111-1111-1111-111111111000"),
            Sku: "SP-001",
            Name: "Пижама женская",
            PlanPerHour: 2.5,
            Description: "Классический костюм для сна",
            Status: SpecificationsStatus.Updated,
            ImagesCount: 3
        );
}

