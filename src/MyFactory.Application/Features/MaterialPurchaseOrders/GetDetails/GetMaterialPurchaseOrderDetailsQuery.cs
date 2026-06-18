using MediatR;
using MyFactory.Application.DTOs.MaterialPurchaseOrders;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.GetDetails;

public sealed record GetMaterialPurchaseOrderDetailsQuery(Guid PurchaseOrderId)
    : IRequest<MaterialPurchaseOrderDetailsDto>;
