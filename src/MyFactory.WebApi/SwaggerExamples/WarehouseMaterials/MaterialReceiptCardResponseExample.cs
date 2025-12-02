using MyFactory.WebApi.Contracts.WarehouseMaterials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.WarehouseMaterials;

public class MaterialReceiptCardResponseExample : IExamplesProvider<MaterialReceiptCardResponse>
{
    public MaterialReceiptCardResponse GetExamples() =>
        new(
            Id: Guid.Parse("aaaaaaaa-0000-0000-0000-000000000001"),
            DocumentNumber: "RC-001",
            DocumentDate: new DateTime(2025, 11, 1),
            SupplierName: "ТексМаркет",
            WarehouseName: "Основной склад",
            TotalAmount: 8750m,
            Status: MaterialReceiptStatus.Draft,
            Comment: "Поступление ткани для зимней коллекции"
        );
}
