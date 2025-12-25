using MediatR;

namespace MyFactory.Application.Features.SalesOrders.UpdateSalesOrder;

public sealed record UpdateSalesOrderCommand(
    Guid OrderId,
    Guid CustomerId,
    DateTime OrderDate
) : IRequest;
