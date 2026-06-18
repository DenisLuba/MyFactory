using MyFactory.WebApi.Contracts.Common;
using MyFactory.WebApi.Contracts.Suppliers;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Suppliers;

public sealed class SupplierListResponseExample : IExamplesProvider<ListResponse<SupplierListItemResponse>>
{
    public ListResponse<SupplierListItemResponse> GetExamples() => new(
        Items:
        [
            new(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"), "ТексМаркет", true),
            new(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"), "Фабрика-Текстиль", true)
        ],
        TotalCount: 2,
        Take: 30,
        Skip: 0,
        HasMore: false);
}
