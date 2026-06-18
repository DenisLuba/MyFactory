using MediatR;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.Confirm;

public sealed record ConfirmMaterialPurchaseOrderCommand(Guid PurchaseOrderId)
    : IRequest;
