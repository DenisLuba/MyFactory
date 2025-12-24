namespace MyFactory.Application.DTOs.Products;

public sealed record ProductBomItemDto
{
    public Guid MaterialId { get; init; }
    public string MaterialName { get; init; } = default!;
    public decimal QtyPerUnit { get; init; }
    public decimal LastUnitPrice { get; init; }
    public decimal TotalCost => QtyPerUnit * LastUnitPrice;
}


