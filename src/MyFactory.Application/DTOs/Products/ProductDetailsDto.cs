namespace MyFactory.Application.DTOs.Products;

public sealed record ProductDetailsDto
{
    public Guid Id { get; init; }
    public string Sku { get; init; } = default!;
    public string Name { get; init; } = default!;
    public int? PlanPerHour { get; init; }
    public string? Description { get; init; }
    public int? Version { get; init; }
    public Domain.Entities.Products.ProductStatus Status { get; init; }

    public decimal MaterialsCost { get; init; }
    public decimal ProductionCost { get; init; }
    public decimal TotalCost => MaterialsCost + ProductionCost;

    public IReadOnlyList<ProductBomItemDto> Bom { get; init; } = [];
    public IReadOnlyList<ProductDepartmentCostDto> ProductionCosts { get; init; } = [];
    public IReadOnlyList<ProductAvailabilityDto> Availability { get; init; } = [];
}