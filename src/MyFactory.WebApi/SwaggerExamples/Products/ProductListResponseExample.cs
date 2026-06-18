using MyFactory.WebApi.Contracts.Common;
using MyFactory.WebApi.Contracts.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public sealed class ProductListResponseExample : IExamplesProvider<ListResponse<ProductListItemResponse>>
{
    public ListResponse<ProductListItemResponse> GetExamples() => new(
        Items:
		[
			new(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"), "SP-001", "Base T-Shirt", null, null, ProductStatus.Active, "Basic model", 2, 1, 520m),
            new(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"), "ROBE-010", "Work Robe", null, null, ProductStatus.Active, "Reinforced", 1, 1, 310m)
        ],
        TotalCount: 128,
        Take: 30,
        Skip: 0,
        HasMore: true);
}
