namespace MyFactory.WebApi.Contracts.ProductionOrders;

public record ProductionOrderMaterialWarehouseResponse(
    Guid WarehouseId,
    string WarehouseName,
    decimal AvailableQty);
