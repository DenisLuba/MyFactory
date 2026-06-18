namespace MyFactory.Application.DTOs.Warehouses;

public sealed class WarehouseStockItemDto
{
    public Guid ItemId { get; init; }          // MaterialId | ProductId
    public string Name { get; init; } = default!;
    public decimal QtyPerPackage { get; init; }
    public decimal? PackageCount { get; init; }
    public decimal TotalQty { get; init; }
    public string? UnitCode { get; init; }     // null для товаров
}