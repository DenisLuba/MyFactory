using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Exceptions;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.Customers;
using MyFactory.Application.DTOs.SalesOrders;

namespace MyFactory.Application.Features.SalesOrders.GetSalesOrderDetails;

public sealed class GetSalesOrderDetailsQueryHandler : IRequestHandler<GetSalesOrderDetailsQuery, SalesOrderDetailsDto>
{
    private readonly IApplicationDbContext _db;

    public GetSalesOrderDetailsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<SalesOrderDetailsDto> Handle(GetSalesOrderDetailsQuery request, CancellationToken cancellationToken)
    {
        var order = await (from o in _db.SalesOrders.AsNoTracking()
                           join c in _db.Customers.AsNoTracking() on o.CustomerId equals c.Id
                           where o.Id == request.OrderId
                           select new
                           {
                               Order = o,
                               Customer = c
                           }).FirstOrDefaultAsync(cancellationToken);
        if (order is null)
            throw new NotFoundException("Sales order not found");

        var items = await (from i in _db.SalesOrderItems.AsNoTracking()
                           join p in _db.Products.AsNoTracking() on i.ProductId equals p.Id
                           where i.SalesOrderId == request.OrderId
                           select new SalesOrderItemDto
                           {
                               Id = i.Id,
                               ProductId = p.Id,
                               ProductName = p.Name,
                               QtyOrdered = i.QtyOrdered
                           }).ToListAsync(cancellationToken);

        return new SalesOrderDetailsDto
        {
            Id = order.Order.Id,
            OrderNumber = order.Order.OrderNumber,
            OrderDate = order.Order.OrderDate,
            Status = order.Order.Status,
            Customer = new CustomerDto
            {
                Id = order.Customer.Id,
                Name = order.Customer.Name
            },
            Items = items
        };
    }
}
