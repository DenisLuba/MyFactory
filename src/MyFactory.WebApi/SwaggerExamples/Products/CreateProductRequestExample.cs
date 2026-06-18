using MyFactory.WebApi.Contracts.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public sealed class CreateProductRequestExample : IExamplesProvider<CreateProductRequest>
{
    public CreateProductRequest GetExamples() => new(
        Name: "Пижама женская",
        ProductTypeId: Guid.Parse("d290f1ee-6c54-4b01-90e6-d701748f0851"),
        Status: ProductStatus.Active,
        PlanPerHour: 3.5m,
        Description: "Легкая хлопковая",
        Version: 1.0m);
}
