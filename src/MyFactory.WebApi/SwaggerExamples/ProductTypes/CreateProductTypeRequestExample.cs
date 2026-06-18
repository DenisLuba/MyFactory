using MyFactory.WebApi.Contracts.ProductTypes;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductTypes;

public sealed class CreateProductTypeRequestExample : IExamplesProvider<CreateProductTypeRequest>
{
    public CreateProductTypeRequest GetExamples() => new("Футболки", "Базовые футболки");
}
