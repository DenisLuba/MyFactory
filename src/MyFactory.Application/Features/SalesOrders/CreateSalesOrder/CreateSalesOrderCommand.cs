using MediatR;

namespace MyFactory.Application.Features.SalesOrders.CreatSalesOrder;

public sealed record CreateSalesOrderCommand(
    Guid CustomerId,
    DateTime OrderDate
) : IRequest<Guid>;
