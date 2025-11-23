using MyFactory.WebApi.Contracts.Inventory;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Inventory;

public class AdjustInventoryRequestExample : IExamplesProvider<AdjustInventoryRequest>
{
    public AdjustInventoryRequest GetExamples() =>
        new(
            MaterialId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            WarehouseId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            NewQuantity: 150.5,
            Reason: "Инвентаризация: расхождение по факту",
            AdjustmentDate: DateTime.Parse("2025-11-15")
        );
}
