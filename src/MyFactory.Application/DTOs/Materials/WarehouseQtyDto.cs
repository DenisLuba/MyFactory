namespace MyFactory.Application.DTOs.Materials;

public sealed record WarehouseQtyDto
{
    public Guid WarehouseId { get; init; }
    public string WarehouseName { get; init; } = default!;
    public decimal Qty { get; init; }
    public string UnitCode { get; init; } = default!;
}
