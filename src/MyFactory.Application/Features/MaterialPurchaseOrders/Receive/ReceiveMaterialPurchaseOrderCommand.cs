using MediatR;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.Receive;

public sealed record ReceiveMaterialPurchaseOrderCommand : IRequest
{
    public Guid PurchaseOrderId { get; init; }
    public Guid ReceivedByUserId { get; init; }
    public IReadOnlyList<ReceiveMaterialPurchaseOrderItem> Items { get; init; } = Array.Empty<ReceiveMaterialPurchaseOrderItem>();
}

public sealed record ReceiveMaterialPurchaseOrderItem
{
    public Guid ItemId { get; init; }
    public IReadOnlyList<ReceiveMaterialPurchaseOrderAllocation> Allocations { get; init; } = Array.Empty<ReceiveMaterialPurchaseOrderAllocation>();
}

public sealed record ReceiveMaterialPurchaseOrderAllocation
{
    public Guid WarehouseId { get; init; }
    public decimal Qty { get; init; }
}

