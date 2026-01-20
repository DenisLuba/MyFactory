using MyFactory.WebApi.Contracts.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public sealed class CreateProductRequestExample : IExamplesProvider<CreateProductRequest>
{
    public CreateProductRequest GetExamples() => new(
        Name: "Пижама женская",
        Status: ProductStatus.Active,
        PlanPerHour: 3.5m,
        Description: "Легкая хлопковая",
        Version: 1.0m);
}
