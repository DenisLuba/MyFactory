using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.SalesOrders;

namespace MyFactory.Application.Features.SalesOrders.GetSalesOrders;

public sealed class GetSalesOrdersQueryHandler
    : IRequestHandler<GetSalesOrdersQuery, IReadOnlyList<SalesOrderListItemDto>>
{
    private readonly IApplicationDbContext _db;

    public GetSalesOrdersQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<SalesOrderListItemDto>> Handle(
        GetSalesOrdersQuery request,
        CancellationToken cancellationToken)
    {
        return await (
            from order in _db.SalesOrders.AsNoTracking()
            join customer in _db.Customers.AsNoTracking()
                on order.CustomerId equals customer.Id
            orderby order.OrderDate descending
            select new SalesOrderListItemDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                CustomerName = customer.Name,
                OrderDate = order.OrderDate,
                Status = order.Status
            }
        ).ToListAsync(cancellationToken);
    }
}
