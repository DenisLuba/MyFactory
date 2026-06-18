using MediatR;
using MyFactory.Application.DTOs.MaterialPurchaseOrders;

namespace MyFactory.Application.Features.MaterialPurchaseOrders.GetSupplierPurchaseOrders;

public sealed record GetSupplierPurchaseOrdersQuery(Guid SupplierId)
    : IRequest<IReadOnlyList<SupplierPurchaseOrderListItemDto>>;
