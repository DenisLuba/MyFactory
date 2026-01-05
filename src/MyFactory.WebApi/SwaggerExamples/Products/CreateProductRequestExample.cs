using MyFactory.Domain.Entities.Products;
using MyFactory.WebApi.Contracts.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public sealed class CreateProductRequestExample : IExamplesProvider<CreateProductRequest>
{
    public CreateProductRequest GetExamples() => new(
        Sku: "SP-001",
        Name: "Пижама женская",
        Status: ProductStatus.Active,
        PlanPerHour: 3);
}
