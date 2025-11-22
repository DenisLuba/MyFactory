namespace MyFactory.WebApi.Contracts.FinishedGoods;

public record FinishedGoodsInventoryResponse(
    Guid SpecificationId,
    Guid WarehouseId,
    int Quantity,
    decimal UnitCost
);
