using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.DTOs.Common;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.SalesOrders;
using MyFactory.Domain.Entities.Orders;

namespace MyFactory.Application.Features.SalesOrders.GetSalesOrders;

public sealed class GetSalesOrdersQueryHandler(IApplicationDbContext db)
        : IRequestHandler<GetSalesOrdersQuery, ListDto<SalesOrderListItemDto>>
{
    public async Task<ListDto<SalesOrderListItemDto>> Handle(
        GetSalesOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var skip = Math.Max(0, request.Skip);
        var take = Math.Clamp(request.Take, 1, 100); 
        var sortBy = request.SortBy?.Trim().ToLowerInvariant();

        var ordersQuery = db.SalesOrders.AsNoTracking();

        if (request.FromDate.HasValue && request.FromDate.Value is DateTime fromDate)
        {
            ordersQuery = ordersQuery
                .Where(o => o.OrderDate >= fromDate);
        }

        if (request.ToDate.HasValue && request.ToDate.Value is DateTime toDate)
        {
            ordersQuery = ordersQuery
                .Where(o => o.OrderDate <= toDate);
        }

        if (request.Status.HasValue && request.Status.Value is SalesOrderStatus status)
        {
            ordersQuery = ordersQuery
                .Where(o => o.Status == status);
        }

        if (request.SearchName is string searchName)
        {
            ordersQuery = ordersQuery
                .Include(o => o.Customer)
                .Where(o => o.Customer != null && EF.Functions.ILike(o.Customer.Name, $"%{searchName}%"));
        }

        var totalCount = await ordersQuery.CountAsync(cancellationToken);
        if (totalCount == 0)
        {
            return new ListDto<SalesOrderListItemDto>
            {
                Items = [],
                TotalCount = 0,
                Skip = skip,
                Take = take,
                HasMore = false
            };
        }

        var orderedQuery = sortBy switch
        {
            "date" => request.SortDesc
                ? ordersQuery.OrderByDescending(o => o.OrderDate)
                    .ThenByDescending(o => o.Id)
                : ordersQuery.OrderBy(o => o.OrderDate)
                    .ThenBy(o => o.Id),

            "number" => request.SortDesc
                ? ordersQuery.OrderByDescending(o => o.OrderNumber)
                    .ThenByDescending(o => o.Id)
                : ordersQuery.OrderBy(o => o.OrderNumber)
                    .ThenBy(o => o.Id),

            "customername" => request.SortDesc
                ? ordersQuery
                    .Include(o => o.Customer)
                    .OrderByDescending(o => o.Customer != null ? o.Customer.Name : string.Empty)
                    .ThenByDescending(o => o.OrderDate)
                : ordersQuery
                    .Include(o => o.Customer)
                    .OrderBy(o => o.Customer != null ? o.Customer.Name : string.Empty)
                    .ThenByDescending(o => o.OrderDate),

            _ => request.SortDesc
                ? ordersQuery.OrderByDescending(o => o.OrderDate).ThenByDescending(o => o.Id)
                : ordersQuery.OrderBy(o => o.OrderDate).ThenBy(o => o.Id)
        };

        var items = await orderedQuery
            .Include(o => o.Customer)
            .Skip(skip)
            .Take(take)
            .Select(o => new SalesOrderListItemDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                CustomerName = o.Customer != null ? o.Customer.Name : string.Empty,
                OrderDate = o.OrderDate,
                Status = o.Status
            })
            .ToListAsync(cancellationToken);

        return new ListDto<SalesOrderListItemDto>
        {
            Items = items,
            TotalCount = totalCount,
            Skip = skip,
            Take = take,
            HasMore = skip + take < totalCount
        };
    }
}
