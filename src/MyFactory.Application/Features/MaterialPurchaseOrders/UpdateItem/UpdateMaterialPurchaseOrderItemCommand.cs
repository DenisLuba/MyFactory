using MediatR;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.UpdateItem;

public sealed record UpdateMaterialPurchaseOrderItemCommand : IRequest
{
    public Guid ItemId { get; init; }
    public decimal Qty { get; init; }
    public decimal UnitPrice { get; init; }
}
