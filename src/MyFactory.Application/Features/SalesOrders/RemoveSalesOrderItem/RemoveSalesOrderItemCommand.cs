using MediatR;

namespace MyFactory.Application.Features.SalesOrders.RemoveSalesOrderItem;

public sealed record RemoveSalesOrderItemCommand(Guid OrderItemId)
    : IRequest;
