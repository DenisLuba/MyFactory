using System;
using MyFactory.WebApi.Contracts.Workshops;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Workshops;

public class WorkshopUpdateResponseExample : IExamplesProvider<WorkshopUpdateResponse>
{
    public WorkshopUpdateResponse GetExamples()
        => new(
            Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            WorkshopStatus.Active
        );
}
