using MyFactory.WebApi.Contracts.ProductionOrders;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.ProductionOrders;

public sealed class ProductionOrderMaterialIssueDetailsResponseExample : IExamplesProvider<ProductionOrderMaterialIssueDetailsResponse>
{
    public ProductionOrderMaterialIssueDetailsResponse GetExamples() => new(
        Material: new ProductionOrderMaterialResponse(
            MaterialId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
            MaterialName: "Ситец",
            RequiredQty: 120,
            AvailableQty: 100,
            MissingQty: 20),
        Warehouses: new List<ProductionOrderMaterialWarehouseResponse>
        {
            new(
                WarehouseId: Guid.Parse("99999999-0000-0000-0000-000000000001"),
                WarehouseName: "Основной",
                AvailableQty: 80),
            new(
                WarehouseId: Guid.Parse("99999999-0000-0000-0000-000000000002"),
                WarehouseName: "Цех №1",
                AvailableQty: 20)
        });
}
