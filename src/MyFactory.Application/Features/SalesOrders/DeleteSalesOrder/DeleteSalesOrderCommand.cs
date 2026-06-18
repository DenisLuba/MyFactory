using MediatR;

namespace MyFactory.Application.Features.SalesOrders.DeleteSalesOrder;

public sealed record DeleteSalesOrderCommand(Guid OrderId) : IRequest;
