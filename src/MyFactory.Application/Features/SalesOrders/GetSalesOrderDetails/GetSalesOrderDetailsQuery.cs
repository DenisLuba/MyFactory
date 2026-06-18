using MediatR;
using MyFactory.Application.DTOs.SalesOrders;

namespace MyFactory.Application.Features.SalesOrders.GetSalesOrderDetails;

public sealed record GetSalesOrderDetailsQuery(Guid OrderId)
    : IRequest<SalesOrderDetailsDto>;
