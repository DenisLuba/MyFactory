using MediatR;
using MyFactory.Application.DTOs.Common;
using MyFactory.Application.DTOs.SalesOrders;
using MyFactory.Domain.Entities.Orders;

namespace MyFactory.Application.Features.SalesOrders.GetSalesOrders;

public sealed record GetSalesOrdersQuery (
    string? SearchName,
    string? SortBy = null,     
    bool SortDesc = false,
    int Skip = 0,
    int Take = 30,
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    SalesOrderStatus? Status = null
) : IRequest<ListDto<SalesOrderListItemDto>>;
