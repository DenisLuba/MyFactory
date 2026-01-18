namespace MyFactory.Application.DTOs.Products;

public sealed record ProductListItemDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public string Sku { get; init; } = default!;
    public Domain.Entities.Products.ProductStatus Status { get; init; }
    public string? Description { get; init; }
    public int? PlanPerHour { get; init; }
    public int? Version { get; init; }
    public decimal CostPrice { get; init; }
}
