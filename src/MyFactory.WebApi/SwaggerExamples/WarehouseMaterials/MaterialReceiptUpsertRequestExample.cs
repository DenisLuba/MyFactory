using MyFactory.WebApi.Contracts.WarehouseMaterials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.WarehouseMaterials;

public class MaterialReceiptUpsertRequestExample : IExamplesProvider<MaterialReceiptUpsertRequest>
{
    public MaterialReceiptUpsertRequest GetExamples() =>
        new(
            DocumentNumber: "RC-010",
            DocumentDate: DateTime.Today,
            SupplierName: "ТексМаркет",
            WarehouseName: "Основной склад",
            TotalAmount: 12345m,
            Comment: "Черновик поступления"
        );
}
