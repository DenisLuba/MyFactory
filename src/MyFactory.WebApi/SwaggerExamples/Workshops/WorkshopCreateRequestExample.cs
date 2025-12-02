using MyFactory.WebApi.Contracts.Workshops;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Workshops;

public class WorkshopCreateRequestExample : IExamplesProvider<WorkshopCreateRequest>
{
    public WorkshopCreateRequest GetExamples()
        => new(
            "Цех лазерной резки",
            WorkshopType.Cutting,
            WorkshopStatus.Active
        );
}
