namespace MyFactory.WebApi.Contracts.ProductionOrders;

public record IssueMaterialLineRequest(
    Guid MaterialId,
    Guid WarehouseId,
    decimal Qty);
