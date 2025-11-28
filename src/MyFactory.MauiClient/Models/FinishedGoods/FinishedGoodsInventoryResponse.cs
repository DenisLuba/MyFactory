namespace MyFactory.MauiClient.Models.FinishedGoods;

public record FinishedGoodsInventoryResponse(
    Guid SpecificationId,
    Guid WarehouseId,
    double Quantity,
    decimal UnitCost);
