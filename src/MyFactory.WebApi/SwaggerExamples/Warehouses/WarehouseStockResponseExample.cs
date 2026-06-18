using MyFactory.WebApi.Contracts.Warehouses;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Warehouses;

public sealed class WarehouseStockResponseExample : IExamplesProvider<IReadOnlyList<WarehouseStockItemResponse>>
{
    public IReadOnlyList<WarehouseStockItemResponse> GetExamples() => new List<WarehouseStockItemResponse>
    {
        new(
            ItemId: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0003"),
            Name: "�����",
            QtyPerPackage: 12.05m,
            PackageCount: 10m,
            TotalQty: 120.5m,
            UnitCode: "m"),
        new(
            ItemId: Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddd0004"),
            Name: "������ 20��",
            QtyPerPackage: 300m,
            PackageCount: null,
            TotalQty: 300m,
            UnitCode: "pcs")
    };
}
