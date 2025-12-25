using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.SalesOrders;

namespace MyFactory.Application.Features.SalesOrders.GetSalesOrderShipments;

public sealed class GetSalesOrderShipmentsQueryHandler : IRequestHandler<GetSalesOrderShipmentsQuery, IReadOnlyList<SalesOrderShipmentDto>>
{
    private readonly IApplicationDbContext _db;

    public GetSalesOrderShipmentsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<SalesOrderShipmentDto>> Handle(
       GetSalesOrderShipmentsQuery request,
       CancellationToken cancellationToken)
    {
        var query =
            from fgmItem in _db.FinishedGoodsMovementItems.AsNoTracking()
            join fgm in _db.FinishedGoodsMovements.AsNoTracking()
                on fgmItem.MovementId equals fgm.Id
            join fg in _db.FinishedGoods.AsNoTracking()
                on new { fgmItem.ProductId, WarehouseId = fgm.ToWarehouseId }
                equals new { fg.ProductId, fg.WarehouseId }
            join prodOrder in _db.ProductionOrders.AsNoTracking()
                on fg.ProductionOrderId equals prodOrder.Id
            join soItem in _db.SalesOrderItems.AsNoTracking()
                on prodOrder.SalesOrderItemId equals soItem.Id
            join warehouse in _db.Warehouses.AsNoTracking()
                on fgm.ToWarehouseId equals warehouse.Id
            join product in _db.Products.AsNoTracking()
                on fgmItem.ProductId equals product.Id
            where soItem.SalesOrderId == request.SalesOrderId
            orderby fgm.MovementDate
            select new SalesOrderShipmentDto
            {
                Id = fgmItem.Id,
                ProductName = product.Name,
                ProductionOrderNumber = prodOrder.ProductionOrderNumber,
                WarehouseName = warehouse.Name,
                Qty = fgmItem.Qty,
                ShippedAt = fgm.MovementDate
            };

        return await query.ToListAsync(cancellationToken);
    }
}
