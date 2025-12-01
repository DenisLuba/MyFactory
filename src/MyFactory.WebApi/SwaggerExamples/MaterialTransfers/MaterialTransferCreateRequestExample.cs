using MyFactory.WebApi.Contracts.MaterialTransfers;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.MaterialTransfers;

public class MaterialTransferCreateRequestExample : IExamplesProvider<MaterialTransferCreateRequest>
{
    public MaterialTransferCreateRequest GetExamples() =>
        new(
            Date: new DateTime(2025, 11, 14),
            ProductionOrder: "PO-18",
            Warehouse: "Основной",
            Items:
            [
                new MaterialTransferItemRequest(
                    Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    "Ткань Ситец",
                    10,
                    "м",
                    185),
                new MaterialTransferItemRequest(
                    Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    "Молния 20 см",
                    50,
                    "шт",
                    13)
            ]);
}
