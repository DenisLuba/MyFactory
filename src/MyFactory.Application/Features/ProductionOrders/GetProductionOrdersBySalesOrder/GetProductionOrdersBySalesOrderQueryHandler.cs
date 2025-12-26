using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.ProductionOrders;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionOrdersBySalesOrder;

public sealed class GetProductionOrdersBySalesOrderQueryHandler : IRequestHandler<GetProductionOrdersBySalesOrderQuery, IReadOnlyList<ProductionOrderListItemDto>>
{
    private readonly IApplicationDbContext _db;

    public GetProductionOrdersBySalesOrderQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<ProductionOrderListItemDto>> Handle(GetProductionOrdersBySalesOrderQuery request, CancellationToken cancellationToken)
    {
        var query =
            from po in _db.ProductionOrders.AsNoTracking()
            join soi in _db.SalesOrderItems.AsNoTracking() on po.SalesOrderItemId equals soi.Id
            join so in _db.SalesOrders.AsNoTracking() on soi.SalesOrderId equals so.Id
            join p in _db.Products.AsNoTracking() on soi.ProductId equals p.Id
            where so.Id == request.SalesOrderId
            orderby po.ProductionOrderNumber
            select new ProductionOrderListItemDto
            {
                Id = po.Id,
                ProductionOrderNumber = po.ProductionOrderNumber,
                SalesOrderNumber = so.OrderNumber,
                ProductName = p.Name,
                QtyPlanned = po.QtyPlanned,
                QtyFinished = po.QtyFinished,
                Status = po.Status
            };
        return await query.ToListAsync(cancellationToken);
    }
}
