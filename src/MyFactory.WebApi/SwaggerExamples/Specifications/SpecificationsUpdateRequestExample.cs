using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Specifications;

namespace MyFactory.WebApi.SwaggerExamples.Specifications;

public class SpecificationsUpdateRequestExample : IExamplesProvider<SpecificationsUpdateRequest>
{
    public SpecificationsUpdateRequest GetExamples() =>
        new(
            Sku: "SP-001",
            Name: "Пижама",
            PlanPerHour: 2.5,
            Description: "Обновленное описание изделия"
        );
}
