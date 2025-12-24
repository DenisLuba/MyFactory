namespace MyFactory.Application.DTOs.Products;

public sealed record ProductAvailabilityDto
{
    public Guid WarehouseId { get; init; }
    public string WarehouseName { get; init; } = default!;
    public int AvailableQty { get; init; }
}