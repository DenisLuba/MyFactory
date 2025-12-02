using MyFactory.WebApi.Contracts.WarehouseMaterials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.WarehouseMaterials;

public class MaterialReceiptListResponseExample : IExamplesProvider<IEnumerable<MaterialReceiptListResponse>>
{
    public IEnumerable<MaterialReceiptListResponse> GetExamples() => new[]
    {
        new MaterialReceiptListResponse(
            Id: Guid.Parse("aaaaaaaa-0000-0000-0000-000000000001"),
            DocumentNumber: "RC-001",
            DocumentDate: new DateTime(2025, 11, 1),
            SupplierName: "ТексМаркет",
            WarehouseName: "Основной склад",
            TotalAmount: 8750m,
            Status: MaterialReceiptStatus.Draft
        ),
        new MaterialReceiptListResponse(
            Id: Guid.Parse("aaaaaaaa-0000-0000-0000-000000000002"),
            DocumentNumber: "RC-002",
            DocumentDate: new DateTime(2025, 11, 3),
            SupplierName: "ТекстильОпт",
            WarehouseName: "Склад №2",
            TotalAmount: 5420m,
            Status: MaterialReceiptStatus.Posted
        )
    };
}
