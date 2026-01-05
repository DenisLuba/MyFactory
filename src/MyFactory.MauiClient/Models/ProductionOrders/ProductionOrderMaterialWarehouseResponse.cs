namespace MyFactory.MauiClient.Models.ProductionOrders;

public record ProductionOrderMaterialWarehouseResponse(
    Guid WarehouseId,
    string WarehouseName,
    decimal AvailableQty);
