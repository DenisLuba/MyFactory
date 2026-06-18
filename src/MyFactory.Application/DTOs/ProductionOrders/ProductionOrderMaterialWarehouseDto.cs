namespace MyFactory.Application.DTOs.ProductionOrders;

public sealed class ProductionOrderMaterialWarehouseDto
{
    public Guid WarehouseId { get; init; }
    public string WarehouseName { get; init; } = null!;
    public decimal AvailableQty { get; init; }
}