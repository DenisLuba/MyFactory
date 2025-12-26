using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionOrders;

public sealed class GetProductionOrdersQueryHandler : IRequestHandler<GetProductionOrdersQuery, IReadOnlyList<ProductionOrderListItemDto>>
{
    private readonly IApplicationDbContext _db;

    public GetProductionOrdersQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<ProductionOrderListItemDto>> Handle(GetProductionOrdersQuery request, CancellationToken cancellationToken)
    {
        var query =
            from po in _db.ProductionOrders.AsNoTracking()
            join soi in _db.SalesOrderItems.AsNoTracking() on po.SalesOrderItemId equals soi.Id
            join so in _db.SalesOrders.AsNoTracking() on soi.SalesOrderId equals so.Id
            join p in _db.Products.AsNoTracking() on soi.ProductId equals p.Id
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
