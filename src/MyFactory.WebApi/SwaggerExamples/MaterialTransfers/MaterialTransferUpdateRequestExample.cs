using MyFactory.WebApi.Contracts.MaterialTransfers;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.MaterialTransfers;

public class MaterialTransferUpdateRequestExample : IExamplesProvider<MaterialTransferUpdateRequest>
{
    public MaterialTransferUpdateRequest GetExamples() =>
        new(
            Date: new DateTime(2025, 11, 12),
            ProductionOrder: "PO-16",
            Warehouse: "Фурнитура",
            Items:
            [
                new MaterialTransferItemRequest(
                    Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    "Пуговица пластик",
                    220,
                    "шт",
                    6.5m)
            ]);
}
