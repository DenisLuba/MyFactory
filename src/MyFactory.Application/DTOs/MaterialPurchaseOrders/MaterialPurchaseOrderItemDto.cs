namespace MyFactory.Application.DTOs.MaterialPurchaseOrders;

public sealed record MaterialPurchaseOrderItemDto
{
    public Guid Id { get; init; }
    public Guid MaterialId { get; init; }
    public string MaterialName { get; init; } = string.Empty;
    public string UnitCode { get; init; } = string.Empty;
    public decimal Qty { get; init; }
    public decimal UnitPrice { get; init; }
}