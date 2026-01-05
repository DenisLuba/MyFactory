using MyFactory.WebApi.Contracts.Suppliers;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Suppliers;

public sealed class SupplierListResponseExample : IExamplesProvider<IReadOnlyList<SupplierListItemResponse>>
{
    public IReadOnlyList<SupplierListItemResponse> GetExamples() => new List<SupplierListItemResponse>
    {
        new(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"), "ТексМаркет", true),
        new(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"), "Фабрика-Текстиль", true)
    };
}
