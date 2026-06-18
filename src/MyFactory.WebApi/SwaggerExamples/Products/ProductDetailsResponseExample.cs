using MyFactory.WebApi.Contracts.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public sealed class ProductDetailsResponseExample : IExamplesProvider<ProductDetailsResponse>
{
    public ProductDetailsResponse GetExamples() => new(
        Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        Sku: "SP-001",
        Name: "Куртка зимняя",
        ProductTypeId: Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffff0006"),
        PlanPerHour: 2,
        Description: "Теплая куртка для зимы",
        Version: 1,
        Status: ProductStatus.Active,
        MaterialsCost: 320m,
        ProductionCost: 200m,
        TotalCost: 520m,
        Bom: new List<ProductBomItemResponse>
        {
            new ProductBomItemResponse(
                MaterialId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
                MaterialName: "Ситец",
                Unit: "шт",
                QtyPerUnit: 1.2m,
                LastUnitPrice: 150m,
                TotalCost: 180m),
            new ProductBomItemResponse(
                MaterialId: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0003"),
                MaterialName: "Хлопок",
                Unit: "м",
                QtyPerUnit: 1m,
                LastUnitPrice: 40m,
                TotalCost: 40m)
        },
        ProductionCosts: new List<ProductDepartmentCostResponse>
        {
            new(
                DepartmentId: Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddd0004"),
                DepartmentName: "Гладильный цех",
                CutCost: 50m,
                SewingCost: 120m,
                PackCost: 30m,
                Expenses: 0m,
                Total: 200m)
        },
        Availability: new List<ProductAvailabilityResponse>
        {
            new ProductAvailabilityResponse(Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeee0005"), "Основной склад готовой продукции", 42m)
        });
}
