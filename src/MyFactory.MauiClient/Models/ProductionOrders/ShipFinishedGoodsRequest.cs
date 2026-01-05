namespace MyFactory.MauiClient.Models.ProductionOrders;

public record ShipFinishedGoodsRequest(
    Guid FromWarehouseId,
    Guid ToWarehouseId,
    int Qty);
