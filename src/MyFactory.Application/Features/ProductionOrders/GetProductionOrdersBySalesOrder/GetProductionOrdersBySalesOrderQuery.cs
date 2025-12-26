using MediatR;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionOrdersBySalesOrder;

public sealed record GetProductionOrdersBySalesOrderQuery(Guid SalesOrderId)
    : IRequest<IReadOnlyList<ProductionOrderListItemDto>>;
