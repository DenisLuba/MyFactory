using MyFactory.WebApi.Contracts.ProductTypes;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductTypes;

public sealed class UpdateProductTypeRequestExample : IExamplesProvider<UpdateProductTypeRequest>
{
    public UpdateProductTypeRequest GetExamples() => new("Футболки", "Обновленное описание типа продукции");
}
