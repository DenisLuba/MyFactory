using MyFactory.WebApi.Contracts.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public sealed class UpdateProductRequestExample : IExamplesProvider<UpdateProductRequest>
{
    public UpdateProductRequest GetExamples() => new(
        Name: "Пижама женская (обновл.)",
        ProductTypeId: Guid.Parse("d3b5f8e2-1c4a-4f6e-9f3b-2a5e6c7d8e9f"),
        PlanPerHour: 4,
        Status: ProductStatus.Active,
        Description: "Обновлен дизайн",
        Version: 2);
}
