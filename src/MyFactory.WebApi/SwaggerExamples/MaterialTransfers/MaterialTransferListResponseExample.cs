using MyFactory.WebApi.Contracts.MaterialTransfers;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.MaterialTransfers;

public class MaterialTransferListResponseExample : IExamplesProvider<MaterialTransferListResponse>
{
    public MaterialTransferListResponse GetExamples() =>
        new(
            TransferId: Guid.Parse("aaaa1111-2222-3333-4444-555555555555"),
            DocumentNumber: "TR-001",
            Date: new DateTime(2025, 11, 10),
            ProductionOrder: "PO-15",
            Warehouse: "Основной",
            TotalAmount: 7850m,
            Status: MaterialTransferStatus.Submitted);
}
