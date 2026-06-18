using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Common;
using MyFactory.Application.DTOs.ProductionOrders;
using MyFactory.Application.DTOs.Products;
using MyFactory.Domain.Entities.Production;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionOrders;

public sealed class GetProductionOrdersQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetProductionOrdersQuery, ListDto<ProductionOrderListItemDto>>
{
    public async Task<ListDto<ProductionOrderListItemDto>> Handle(GetProductionOrdersQuery request, CancellationToken cancellationToken)
    {
        var skip = Math.Max(0, request.Skip);
        var take = Math.Clamp(request.Take, 1, 100);
        var sortBy = request.SortBy?.Trim().ToLowerInvariant();

        var ordersQuery = db.ProductionOrders.AsNoTracking();

        if (request.FromDate.HasValue && request.FromDate.Value is DateTime fromDate)
        {
            ordersQuery = ordersQuery
                .Where(o => o.CreatedAt >= fromDate);
        }

        if (request.ToDate.HasValue && request.ToDate.Value is DateTime toDate)
        {
            ordersQuery = ordersQuery
                .Where(o => o.CreatedAt <= toDate);
        }

        if (request.Status.HasValue && request.Status.Value is ProductionOrderStatus status)
        {
            ordersQuery = ordersQuery
                .Where(o => o.Status == status);
        }

        if (request.SearchSaleOrderId.HasValue && request.SearchSaleOrderId.Value is Guid saleOrderId)
        {
            ordersQuery = from po in ordersQuery
                          join soi in db.SalesOrderItems.AsNoTracking() on po.SalesOrderItemId equals soi.Id
                          where soi.SalesOrderId == saleOrderId
                          select po;
        }

        if (!string.IsNullOrWhiteSpace(request.SearchProductionOrderNumber)
            && request.SearchProductionOrderNumber.All(char.IsDigit))
        {
            var search = request.SearchProductionOrderNumber.Trim();
            ordersQuery = ordersQuery
                .Where(po => po.ProductionOrderNumber.ToString().Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(request.SearchSaleOrderNumber)
            && request.SearchSaleOrderNumber.All(char.IsDigit))
        {
            var search = request.SearchSaleOrderNumber.Trim();
            ordersQuery = from po in ordersQuery
                          join soi in db.SalesOrderItems.AsNoTracking() on po.SalesOrderItemId equals soi.Id
                          join so in db.SalesOrders.AsNoTracking() on soi.SalesOrderId equals so.Id
                          where so.OrderNumber.ToString().Contains(search)
                          select po;
        }

        // the sales order search by name of customer 
        if (request.SearchCustomerName is string searchName && !string.IsNullOrWhiteSpace(searchName))
        {
            ordersQuery = from po in ordersQuery
                          join soi in db.SalesOrderItems.AsNoTracking() on po.SalesOrderItemId equals soi.Id
                          join so in db.SalesOrders.AsNoTracking() on soi.SalesOrderId equals so.Id
                          join c in db.Customers.AsNoTracking() on so.CustomerId equals c.Id
                          where EF.Functions.ILike(c.Name, $"%{searchName.Trim()}%")
                          select po;
        }

        if (request.SearchProductName is string searchProductName && !string.IsNullOrWhiteSpace(searchProductName))
        {
            ordersQuery = from po in ordersQuery
                          join soi in db.SalesOrderItems.AsNoTracking() on po.SalesOrderItemId equals soi.Id
                          join p in db.Products.AsNoTracking() on soi.ProductId equals p.Id
                          where EF.Functions.ILike(p.Name, $"%{searchProductName.Trim()}%")
                          select po;
        }

        var totalCount = await ordersQuery.CountAsync(cancellationToken);
        if (totalCount == 0)
        {
            return new ListDto<ProductionOrderListItemDto>
            {
                Items = [],
                TotalCount = 0,
                Skip = skip,
                Take = take,
                HasMore = false
            };
        }

        // sort sales orders by creation date, order number, or customer name
        var orderedQuery = sortBy switch
        {
            "date" => request.SortDesc
                ? ordersQuery.OrderByDescending(o => o.CreatedAt)
                    .ThenByDescending(o => o.Id)
                : ordersQuery.OrderBy(o => o.CreatedAt)
                    .ThenBy(o => o.Id),

            "number" => request.SortDesc
                ? ordersQuery.OrderByDescending(o => o.ProductionOrderNumber)
                    .ThenByDescending(o => o.Id)
                : ordersQuery.OrderBy(o => o.ProductionOrderNumber)
                    .ThenBy(o => o.Id),

            "customername" => request.SortDesc
                ? from po in ordersQuery
                  join soi in db.SalesOrderItems.AsNoTracking() on po.SalesOrderItemId equals soi.Id
                  join so in db.SalesOrders.AsNoTracking() on soi.SalesOrderId equals so.Id
                  join c in db.Customers.AsNoTracking() on so.CustomerId equals c.Id
                  orderby c.Name descending
                  select po

                : from po in ordersQuery
                  join soi in db.SalesOrderItems.AsNoTracking() on po.SalesOrderItemId equals soi.Id
                  join so in db.SalesOrders.AsNoTracking() on soi.SalesOrderId equals so.Id
                  join c in db.Customers.AsNoTracking() on so.CustomerId equals c.Id
                  orderby c.Name 
                  select po,

            "salesordernumber" => request.SortDesc
                ? from po in ordersQuery
                  join soi in db.SalesOrderItems.AsNoTracking() on po.SalesOrderItemId equals soi.Id
                  join so in db.SalesOrders.AsNoTracking() on soi.SalesOrderId equals so.Id
                  orderby so.OrderNumber descending
                  select po

                : from po in ordersQuery
                  join soi in db.SalesOrderItems.AsNoTracking() on po.SalesOrderItemId equals soi.Id
                  join so in db.SalesOrders.AsNoTracking() on soi.SalesOrderId equals so.Id
                  orderby so.OrderNumber 
                  select po,

            "productname" => request.SortDesc
                ? from po in ordersQuery
                  join soi in db.SalesOrderItems.AsNoTracking() on po.SalesOrderItemId equals soi.Id
                  join p in db.Products.AsNoTracking() on soi.ProductId equals p.Id
                  orderby p.Name descending
                  select po

                : from po in ordersQuery
                  join soi in db.SalesOrderItems.AsNoTracking() on po.SalesOrderItemId equals soi.Id
                  join p in db.Products.AsNoTracking() on soi.ProductId equals p.Id
                  orderby p.Name 
                  select po,

            _ => request.SortDesc
                ? ordersQuery.OrderByDescending(o => o.CreatedAt).ThenByDescending(o => o.Id)
                : ordersQuery.OrderBy(o => o.CreatedAt).ThenBy(o => o.Id)
        };

        var items = await (
            from po in orderedQuery.Skip(skip).Take(take)
            join soi in db.SalesOrderItems.AsNoTracking() on po.SalesOrderItemId equals soi.Id
            join so in db.SalesOrders.AsNoTracking() on soi.SalesOrderId equals so.Id
            join p in db.Products.AsNoTracking() on soi.ProductId equals p.Id
            join c in db.Customers.AsNoTracking() on so.CustomerId equals c.Id
            select new ProductionOrderListItemDto
            {
                Id = po.Id,
                CustomerName = c.Name,
                ProductionOrderNumber = po.ProductionOrderNumber,
                SalesOrderNumber = so.OrderNumber,
                ProductName = p.Name,
                QtyPlanned = po.QtyPlanned,
                QtyFinished = po.QtyFinished,
                Status = po.Status
            }).ToListAsync(cancellationToken);

        return new ListDto<ProductionOrderListItemDto>
        {
            Items = items,
            TotalCount = totalCount,
            Skip = skip,
            Take = take,
            HasMore = skip + items.Count < totalCount
        };
    }
}
