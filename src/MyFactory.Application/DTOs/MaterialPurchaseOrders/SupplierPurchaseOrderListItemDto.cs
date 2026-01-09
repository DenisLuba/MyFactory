using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Application.DTOs.MaterialPurchaseOrders;

public sealed record SupplierPurchaseOrderListItemDto
{
    public Guid Id { get; init; }
    public DateTime OrderDate { get; init; }
    public PurchaseOrderStatus Status { get; init; }
    public int ItemsCount { get; init; }
    public decimal TotalAmount { get; init; }
}




