using MediatR;

namespace MyFactory.Application.Features.SalesOrders.CompleteSalesOrder;

public sealed record CompleteSalesOrderCommand(Guid OrderId) : IRequest;
