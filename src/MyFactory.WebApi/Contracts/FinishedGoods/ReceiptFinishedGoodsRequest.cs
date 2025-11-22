namespace MyFactory.WebApi.Contracts.FinishedGoods;

public record ReceiptFinishedGoodsRequest(
    Guid SpecificationId,
    Guid WarehouseId,
    int Quantity,
    decimal UnitCost,
    DateTime? ProductionDate = null
);
