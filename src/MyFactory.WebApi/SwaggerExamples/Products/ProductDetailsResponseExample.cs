using MyFactory.WebApi.Contracts.Products;
using Swashbuckle.AspNetCore.Filters;

namespace MyFactory.WebApi.SwaggerExamples.Products;

public sealed class ProductDetailsResponseExample : IExamplesProvider<ProductDetailsResponse>
{
    public ProductDetailsResponse GetExamples() => new(
        Id: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"),
        Sku: "SP-001",
        Name: "Пижама женская",
        PlanPerHour: 2,
        Description: "Легкая хлопковая",
        Version: 1,
        Status: ProductStatus.Active,
        MaterialsCost: 320m,
        ProductionCost: 200m,
        TotalCost: 520m,
        Bom: new List<ProductBomItemResponse>
        {
            new(
                MaterialId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb0002"),
                MaterialName: "Ситец",
                QtyPerUnit: 1.2m,
                LastUnitPrice: 150m,
                TotalCost: 180m),
            new(
                MaterialId: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccc0003"),
                MaterialName: "Молния",
                QtyPerUnit: 1m,
                LastUnitPrice: 40m,
                TotalCost: 40m)
        },
        ProductionCosts: new List<ProductDepartmentCostResponse>
        {
            new(
                DepartmentId: Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddd0004"),
                DepartmentName: "Швейный",
                CutCost: 50m,
                SewingCost: 120m,
                PackCost: 30m,
                Expenses: 0m,
                Total: 200m)
        },
        Availability: new List<ProductAvailabilityResponse>
        {
            new(Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeee0005"), "Склад готовой продукции", 42)
        });
}
