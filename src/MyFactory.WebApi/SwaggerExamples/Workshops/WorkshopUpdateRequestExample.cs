using MyFactory.WebApi.Contracts.Workshops;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Workshops;

public class WorkshopUpdateRequestExample : IExamplesProvider<WorkshopUpdateRequest>
{
    public WorkshopUpdateRequest GetExamples()
        => new(
            "Пошив",
            WorkshopType.Sewing,
            WorkshopStatus.Active
        );
}
