using MyFactory.WebApi.Contracts.Inventory;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Inventory;

public class CreateInventoryReceiptRequestExample : IExamplesProvider<CreateInventoryReceiptRequest>
{
    public CreateInventoryReceiptRequest GetExamples() =>
        new(
            WarehouseId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            ReceiptDate: DateTime.Parse("2025-11-15"),
            ReferenceNumber: "ПРИХ-001",
            Items:
            [
                new(
                    MaterialId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Quantity: 100.5,
                    UnitPrice: 180.75m,
                    BatchNumber: "BATCH-2025-11"
                ),
                new(
                    MaterialId: Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Quantity: 50.0,
                    UnitPrice: 25.50m
                    // BatchNumber не указан - демонстрация опционального параметра
                )
            ]
        );
}

