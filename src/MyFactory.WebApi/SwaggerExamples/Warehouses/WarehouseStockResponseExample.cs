using MyFactory.WebApi.Contracts.Warehouses;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Warehouses;

public sealed class WarehouseStockResponseExample : IExamplesProvider<IReadOnlyList<WarehouseStockItemResponse>>
{
    public IReadOnlyList<WarehouseStockItemResponse> GetExamples() => new List<WarehouseStockItemResponse>
    {
        new(
            ItemId: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0003"),
            Name: "Ситец",
            Qty: 120.5m,
            UnitCode: "m"),
        new(
            ItemId: Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddd0004"),
            Name: "Молния 20см",
            Qty: 300m,
            UnitCode: "pcs")
    };
}
