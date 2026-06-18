namespace MyFactory.Application.DTOs.SalesOrders;

public sealed class SalesOrderShipmentDto
{
    public Guid Id { get; init; }
    public string ProductName { get; init; } = null!;
    public int ProductionOrderNumber { get; init; }
    public string WarehouseName { get; init; } = null!;
    public decimal Qty { get; init; }
    public DateTime ShippedAt { get; init; }
}
