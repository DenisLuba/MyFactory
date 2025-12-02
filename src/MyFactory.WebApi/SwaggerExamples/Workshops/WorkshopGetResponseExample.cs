using System;
using MyFactory.WebApi.Contracts.Workshops;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Workshops;

public class WorkshopGetResponseExample : IExamplesProvider<WorkshopGetResponse>
{
    public WorkshopGetResponse GetExamples()
        => new(
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            "Крой",
            WorkshopType.Cutting,
            WorkshopStatus.Active
        );
}
