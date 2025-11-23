namespace MyFactory.WebApi.Contracts.FinishedGoods;

public record FinishedGoodsInventoryResponse(
    Guid SpecificationId,
    Guid WarehouseId,
    double Quantity,
    decimal UnitCost
);
