namespace MyFactory.Application.DTOs.Warehouses;

public sealed class WarehouseStockItemDto
{
    public Guid ItemId { get; init; }          // MaterialId | ProductId
    public string Name { get; init; } = default!;
    public decimal Qty { get; init; }
    public string? UnitCode { get; init; }     // null для товаров
}