using MyFactory.WebApi.Contracts.WarehouseMaterials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.WarehouseMaterials;

public class MaterialReceiptLineUpsertResponseExample : IExamplesProvider<MaterialReceiptLineUpsertResponse>
{
    public MaterialReceiptLineUpsertResponse GetExamples()
        => new(
            ReceiptId: Guid.Parse("aaaaaaaa-0000-0000-0000-000000000001"),
            Line: new MaterialReceiptLineResponse(
                Id: Guid.Parse("bbbbbbbb-0000-0000-0000-000000000010"),
                MaterialId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
                MaterialName: "Ткань Ситец",
                Quantity: 10,
                Unit: "м",
                Price: 200m,
                Amount: 2000m
            ),
            Status: MaterialReceiptStatus.LineAdded
        );
}
