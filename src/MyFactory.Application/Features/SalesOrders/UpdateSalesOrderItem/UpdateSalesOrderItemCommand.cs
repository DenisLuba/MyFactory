using MediatR;

namespace MyFactory.Application.Features.SalesOrders.UpdateSalesOrderItem;

public sealed record UpdateSalesOrderItemCommand(
    Guid OrderItemId,
    decimal Qty
) : IRequest;
