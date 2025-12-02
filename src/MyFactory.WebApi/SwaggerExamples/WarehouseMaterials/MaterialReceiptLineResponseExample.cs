using MyFactory.WebApi.Contracts.WarehouseMaterials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.WarehouseMaterials;

public class MaterialReceiptLineResponseExample : IExamplesProvider<IEnumerable<MaterialReceiptLineResponse>>
{
    public IEnumerable<MaterialReceiptLineResponse> GetExamples() => new[]
    {
        new MaterialReceiptLineResponse(
            Id: Guid.Parse("bbbbbbbb-0000-0000-0000-000000000001"),
            MaterialId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
            MaterialName: "Ткань Ситец",
            Quantity: 50,
            Unit: "м",
            Price: 150m,
            Amount: 7500m
        ),
        new MaterialReceiptLineResponse(
            Id: Guid.Parse("bbbbbbbb-0000-0000-0000-000000000002"),
            MaterialId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
            MaterialName: "Нитки хлопковые",
            Quantity: 200,
            Unit: "шт",
            Price: 2.5m,
            Amount: 500m
        )
    };
}
