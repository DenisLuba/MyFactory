using MyFactory.Domain.Entities.Materials;

namespace MyFactory.Application.DTOs.MaterialPurchaseOrders;

public sealed record MaterialPurchaseOrderDetailsDto
{
    public Guid Id { get; init; }
    public Guid SupplierId { get; init; }
    public string SupplierName { get; init; } = string.Empty;
    public DateTime OrderDate { get; init; }
    public PurchaseOrderStatus Status { get; init; }
    public IReadOnlyList<MaterialPurchaseOrderItemDto> Items { get; init; } = Array.Empty<MaterialPurchaseOrderItemDto>();
}