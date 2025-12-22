namespace MyFactory.Application.DTOs.Products;

public sealed record ProductListItemDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public decimal CostPrice { get; init; }
}
