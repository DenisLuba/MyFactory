using MediatR;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.RemoveItem;

public sealed record RemoveMaterialPurchaseOrderItemCommand : IRequest
{
    public Guid ItemId { get; init; }
}
