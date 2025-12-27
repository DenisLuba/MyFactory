namespace MyFactory.Application.DTOs.ProductionOrders;

public sealed class ProductionOrderShipmentDto
{
    public Guid WarehouseId { get; init; }
    public string WarehouseName { get; init; } = null!;
    public int Qty { get; init; }
    public DateTime ShipmentDate { get; init; }
}
