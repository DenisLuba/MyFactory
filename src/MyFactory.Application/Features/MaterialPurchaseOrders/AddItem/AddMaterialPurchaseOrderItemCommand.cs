using MediatR;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.AddItem;

public sealed record AddMaterialPurchaseOrderItemCommand : IRequest
{
    public Guid PurchaseOrderId { get; init; }
    public Guid MaterialId { get; init; }
    public decimal Qty { get; init; }
    public decimal UnitPrice { get; init; }
}
