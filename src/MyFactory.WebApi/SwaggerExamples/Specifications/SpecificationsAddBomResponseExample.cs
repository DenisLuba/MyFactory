using Swashbuckle.AspNetCore.Filters;
using MyFactory.WebApi.Contracts.Specifications;

namespace MyFactory.WebApi.SwaggerExamples.Specifications;

public class SpecificationsAddBomResponseExample : IExamplesProvider<SpecificationsAddBomResponse>
{
    public SpecificationsAddBomResponse GetExamples() =>
        new(
            SpecificationId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            BomItemId: Guid.NewGuid(),
            Status: SpecificationsStatus.BomAdded
        );
}
