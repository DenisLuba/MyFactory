using MediatR;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.Receive;

public sealed record ReceiveMaterialPurchaseOrderCommand : IRequest
{
    public Guid PurchaseOrderId { get; init; }
    public Guid ReceivedByUserId { get; init; }
    public IReadOnlyList<ReceiveMaterialPurchaseOrderAllocation> Allocations { get; set; } = [];
}

public sealed record ReceiveMaterialPurchaseOrderAllocation
{
    public Guid WarehouseId { get; init; }
    public decimal? ShippingCost { get; init; }
    public IReadOnlyList<ReceiveMaterialPurchaseOrderItem> MateialItems { get; set; } = [];
}

public sealed record ReceiveMaterialPurchaseOrderItem
{
    public Guid MaterialId { get; init; }
    public decimal QtyPerPackage { get; init; }
    public decimal? PackageCount { get; init; }
}