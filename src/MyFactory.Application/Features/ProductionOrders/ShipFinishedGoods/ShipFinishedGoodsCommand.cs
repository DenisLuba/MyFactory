using MediatR;

namespace MyFactory.Application.Features.ProductionOrders.ShipFinishedGoods;

public sealed class ShipFinishedGoodsCommand : IRequest
{
    public Guid ProductionOrderId { get; init; }
    public Guid FromWarehouseId { get; init; }
    public Guid ToWarehouseId { get; init; }
    public decimal QtyPerPackage { get; init; }
    public decimal? PackageCount { get; init; }
}

