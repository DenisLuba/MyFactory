using MyFactory.WebApi.Contracts.MaterialTransfers;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.MaterialTransfers;

public class MaterialTransferCardResponseExample : IExamplesProvider<MaterialTransferCardResponse>
{
    public MaterialTransferCardResponse GetExamples() =>
        new(
            TransferId: Guid.Parse("aaaa1111-2222-3333-4444-555555555555"),
            DocumentNumber: "TR-001",
            Date: new DateTime(2025, 11, 10),
            ProductionOrder: "PO-15",
            Warehouse: "Основной",
            TotalAmount: 7850m,
            Status: MaterialTransferStatus.Submitted,
            Items:
            [
                new MaterialTransferItemDto(
                    Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    "Ткань Ситец",
                    15,
                    "м",
                    180,
                    2700),
                new MaterialTransferItemDto(
                    Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    "Молния 20 см",
                    100,
                    "шт",
                    12,
                    1200)
            ]);
}
