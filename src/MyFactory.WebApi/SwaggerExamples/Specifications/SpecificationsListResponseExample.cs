using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Specifications;

namespace MyFactory.WebApi.SwaggerExamples.Specifications;

public class SpecificationsListResponseExample : IExamplesProvider<IEnumerable<SpecificationsListResponse>>
{
    public IEnumerable<SpecificationsListResponse> GetExamples() => new[]
    {
        new SpecificationsListResponse(
            Id: Guid.Parse("11111111-1111-1111-1111-111111111000"),
            Sku: "SP-001",
            Name: "Пижама женская",
            PlanPerHour: 2.5,
            Status: SpecificationsStatus.Updated,
            ImagesCount: 3
        ),
        new SpecificationsListResponse(
            Id: Guid.Parse("22222222-2222-2222-2222-222222222000"),
            Sku: "SP-002",
            Name: "Халат махровый",
            PlanPerHour: 1.3,
            Status: SpecificationsStatus.Created,
            ImagesCount: 1
        )
    };
}
