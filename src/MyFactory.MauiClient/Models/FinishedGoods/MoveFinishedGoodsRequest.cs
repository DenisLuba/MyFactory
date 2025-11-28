namespace MyFactory.MauiClient.Models.FinishedGoods;

public record MoveFinishedGoodsRequest(
    Guid SpecificationId,
    Guid FromWarehouseId,
    Guid ToWarehouseId,
    int Quantity,
    string? Reason = null);
