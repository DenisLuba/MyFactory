using MyFactory.WebApi.Contracts.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public sealed class UpdateProductRequestExample : IExamplesProvider<UpdateProductRequest>
{
    public UpdateProductRequest GetExamples() => new(
        Name: "Пижама женская (обновл.)",
        PlanPerHour: 4,
        Status: ProductStatus.Active,
        Description: "Обновлен дизайн",
        Version: 2);
}
