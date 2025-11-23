using MyFactory.WebApi.Contracts.Inventory;
using MyFactory.WebApi.Contracts.Materials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Inventory;

public class InventoryItemResponseExample : IExamplesProvider<InventoryItemResponse>
{
    public InventoryItemResponse GetExamples() =>
        new(
            MaterialId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            MaterialName: "Ткань Ситец",
            WarehouseId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Quantity: 120.5,
            Unit: Units.Meter,
            AvgPrice: 180.75m,
            ReservedQty: 15.0
        );
}
