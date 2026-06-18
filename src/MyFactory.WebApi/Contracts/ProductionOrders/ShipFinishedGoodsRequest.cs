namespace MyFactory.WebApi.Contracts.ProductionOrders;

public record ShipFinishedGoodsRequest(
    Guid FromWarehouseId,
    Guid ToWarehouseId,
    decimal QtyPerPackage,
    decimal? PackageCount = null);
