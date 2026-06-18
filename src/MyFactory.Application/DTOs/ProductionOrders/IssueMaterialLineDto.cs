namespace MyFactory.Application.DTOs.ProductionOrders;

public sealed class IssueMaterialLineDto
{
    public Guid MaterialId { get; init; }
    public Guid WarehouseId { get; init; }
    public decimal Qty { get; init; }
}

