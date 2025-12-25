using MediatR;

namespace MyFactory.Application.Features.SalesOrders.CancelSalesOrder;

public sealed record CancelSalesOrderCommand(Guid OrderId) : IRequest;
