using MyFactory.WebApi.Contracts.Materials;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Materials;

public sealed class MaterialDetailsResponseExample : IExamplesProvider<MaterialDetailsResponse>
{
    public MaterialDetailsResponse GetExamples() => new(
        Id: Guid.Parse("33333333-3333-3333-3333-333333333333"),
        Name: "Ситец",
        MaterialType: "Ткань",
        UnitCode: "м",
        Color: "Белый",
        TotalQty: 180,
        Warehouses: new List<WarehouseQtyResponse>
        {
            new(
                WarehouseId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                WarehouseName: "Основной",
                Qty: 120,
                UnitCode: "м"),
            new(
                WarehouseId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                WarehouseName: "Цех №1",
                Qty: 60,
                UnitCode: "м")
        },
        PurchaseHistory: new List<MaterialPurchaseHistoryItemResponse>
        {
            new(
                SupplierId: Guid.Parse("44444444-4444-4444-4444-444444444444"),
                SupplierName: "Текстиль+",
                Qty: 100,
                UnitPrice: 180,
                PurchaseDate: new DateTime(2024, 1, 12)),
            new(
                SupplierId: Guid.Parse("55555555-5555-5555-5555-555555555555"),
                SupplierName: "Иванов ИП",
                Qty: 50,
                UnitPrice: 175,
                PurchaseDate: new DateTime(2023, 11, 3))
        });
}
