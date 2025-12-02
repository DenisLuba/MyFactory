using System;
using MyFactory.WebApi.Contracts.Workshops;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Workshops;

public class WorkshopCreateResponseExample : IExamplesProvider<WorkshopCreateResponse>
{
    public WorkshopCreateResponse GetExamples()
        => new(
            Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
            WorkshopStatus.Active
        );
}
