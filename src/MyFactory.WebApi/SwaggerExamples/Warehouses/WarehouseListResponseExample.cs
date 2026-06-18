using MyFactory.Domain.Entities.Inventory;
using MyFactory.WebApi.Contracts.Warehouses;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Warehouses;

public sealed class WarehouseListResponseExample : IExamplesProvider<IReadOnlyList<WarehouseListItemResponse>>
{
    public IReadOnlyList<WarehouseListItemResponse> GetExamples() => new List<WarehouseListItemResponse>
    {
        new(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"), "Склад материалов", WarehouseType.Materials, true),
        new(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"), "Склад готовой продукции", WarehouseType.FinishedGoods, true)
    };
}
