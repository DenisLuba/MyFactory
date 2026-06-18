using MyFactory.WebApi.Contracts.ProductTypes;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductTypes;

public sealed class ProductTypeListResponseExample : IExamplesProvider<IReadOnlyList<ProductTypeResponse>>
{
    public IReadOnlyList<ProductTypeResponse> GetExamples() => new List<ProductTypeResponse>
    {
        new(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"), "Футболки", "Базовые футболки"),
        new(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"), "Платья", "Летние и вечерние платья")
    };
}
