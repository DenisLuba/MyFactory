using MediatR;

namespace MyFactory.Application.Features.SalesOrders.StartSalesOrder;

public sealed record StartSalesOrderCommand(Guid OrderId) : IRequest;
