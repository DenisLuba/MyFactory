using MediatR;

namespace MyFactory.Application.Features.SalesOrders.AddSalesOrderItem;

public sealed record AddSalesOrderItemCommand(
    Guid OrderId,
    Guid ProductId,
    decimal Qty
) : IRequest<Guid>;
