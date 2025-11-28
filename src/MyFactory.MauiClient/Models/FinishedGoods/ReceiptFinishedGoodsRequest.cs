namespace MyFactory.MauiClient.Models.FinishedGoods;

public record ReceiptFinishedGoodsRequest(
    Guid SpecificationId,
    Guid WarehouseId,
    int Quantity,
    decimal UnitCost,
    DateTime? ProductionDate = null);
