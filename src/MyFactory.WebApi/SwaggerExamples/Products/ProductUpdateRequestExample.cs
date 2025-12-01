using MyFactory.WebApi.Contracts.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public class ProductUpdateRequestExample : IExamplesProvider<ProductUpdateRequest>
{
    public ProductUpdateRequest GetExamples() => new(
        "SP-001",
        "Пижама женская",
        2.7,
        "Обновлённое описание изделия"
    );
}
