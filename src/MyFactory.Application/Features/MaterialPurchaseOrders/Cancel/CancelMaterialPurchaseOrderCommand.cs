using MediatR;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.Cancel;

public sealed record CancelMaterialPurchaseOrderCommand : IRequest
{
    public Guid PurchaseOrderId { get; init; }
}
