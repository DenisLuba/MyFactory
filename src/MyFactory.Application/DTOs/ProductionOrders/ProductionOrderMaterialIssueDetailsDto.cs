namespace MyFactory.Application.DTOs.ProductionOrders;

public sealed class ProductionOrderMaterialIssueDetailsDto
{
    public ProductionOrderMaterialDto Material { get; init; } = null!;
    public IReadOnlyList<ProductionOrderMaterialWarehouseDto> Warehouses { get; init; } = [];
}


