using MediatR;
using MyFactory.Application.DTOs.SalesOrders;

namespace MyFactory.Application.Features.SalesOrders.GetSalesOrders;

public sealed record GetSalesOrdersQuery : IRequest<IReadOnlyList<SalesOrderListItemDto>>;
