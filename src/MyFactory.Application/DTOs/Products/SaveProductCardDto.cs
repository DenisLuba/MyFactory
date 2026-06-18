namespace MyFactory.Application.DTOs.Products;

public sealed class SaveProductCardDto
{
    public ProductEditDto Product { get; init; } = null!;
    public List<ProductMaterialDto> Materials { get; init; } = [];
    public List<ProductDepartmentCostDto> ProductionCosts { get; init; } = [];
}
