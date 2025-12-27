using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFactory.Application.Common.Interfaces;
using MyFactory.Application.DTOs.ProductionOrders;

namespace MyFactory.Application.Features.ProductionOrders.GetProductionOrderShipments;

public sealed class GetProductionOrderShipmentsQueryHandler : IRequestHandler<GetProductionOrderShipmentsQuery, IReadOnlyList<ProductionOrderShipmentDto>>
{
    private readonly IApplicationDbContext _db;

    public GetProductionOrderShipmentsQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<ProductionOrderShipmentDto>> Handle(GetProductionOrderShipmentsQuery request, CancellationToken cancellationToken)
    {
        var query =
            from fg in _db.FinishedGoods.AsNoTracking()
            join w in _db.Warehouses.AsNoTracking() on fg.WarehouseId equals w.Id
            where fg.ProductionOrderId == request.ProductionOrderId
            group fg by new { fg.WarehouseId, w.Name } into g
            select new ProductionOrderShipmentDto
            {
                WarehouseId = g.Key.WarehouseId,
                WarehouseName = g.Key.Name,
                Qty = g.Sum(x => x.Qty),
                ShipmentDate = g.Max(x => x.CreatedAt)
            };

        return await query.ToListAsync(cancellationToken);
    }
}
