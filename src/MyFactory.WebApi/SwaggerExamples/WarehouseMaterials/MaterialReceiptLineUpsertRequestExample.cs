using MyFactory.WebApi.Contracts.WarehouseMaterials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.WarehouseMaterials;

public class MaterialReceiptLineUpsertRequestExample : IExamplesProvider<MaterialReceiptLineUpsertRequest>
{
    public MaterialReceiptLineUpsertRequest GetExamples() =>
        new(
            MaterialId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Quantity: 10,
            Unit: "м",
            Price: 200m
        );
}
