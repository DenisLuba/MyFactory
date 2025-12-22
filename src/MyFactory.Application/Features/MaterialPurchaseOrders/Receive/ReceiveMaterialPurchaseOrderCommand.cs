using MediatR;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.Receive;

public sealed record ReceiveMaterialPurchaseOrderCommand : IRequest
{
    public Guid PurchaseOrderId { get; init; }
    public Guid WarehouseId { get; init; }
    public DateOnly ReceiveDate { get; init; }
    public Guid ReceivedByUserId { get; init; }
}
