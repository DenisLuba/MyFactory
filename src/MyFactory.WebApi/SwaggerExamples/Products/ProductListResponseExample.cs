using MyFactory.WebApi.Contracts.Products;
using MyFactory.Domain.Entities.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public sealed class ProductListResponseExample : IExamplesProvider<IReadOnlyList<ProductListItemResponse>>
{
    public IReadOnlyList<ProductListItemResponse> GetExamples() => new List<ProductListItemResponse>
    {
        new(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"), "SP-001", "Пижама женская", ProductStatus.Active, "Легкая хлопковая", 2, 1, 520m),
        new(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"), "ROBE-010", "Халат махровый", ProductStatus.Active, "Плотный", 1, 1, 310m)
    };
}
