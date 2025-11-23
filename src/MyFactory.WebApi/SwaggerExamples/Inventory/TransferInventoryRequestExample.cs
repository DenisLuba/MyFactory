using MyFactory.WebApi.Contracts.Inventory;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Inventory;

public class TransferInventoryRequestExample : IExamplesProvider<TransferInventoryRequest>
{
    public TransferInventoryRequest GetExamples() =>
        new(
            MaterialId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            FromWarehouseId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            ToWarehouseId: Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Quantity: 50.5,
            Reason: "Перемещение между складами для выполнения заказа",
            TransferDate: DateTime.Parse("2025-11-15")
        );
}
