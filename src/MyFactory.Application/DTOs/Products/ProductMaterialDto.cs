namespace MyFactory.Application.DTOs.Products;

public sealed class ProductMaterialDto
{
    public Guid? Id { get; init; }        // null → add
    public Guid MaterialId { get; init; }
    public decimal QtyPerUnit { get; init; }

    // только для UI
    public string? MaterialName { get; init; }
    public string? UnitCode { get; init; }
}
