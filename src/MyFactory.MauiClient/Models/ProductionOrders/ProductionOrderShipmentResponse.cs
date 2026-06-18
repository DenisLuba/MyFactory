namespace MyFactory.MauiClient.Models.ProductionOrders;

public record ProductionOrderShipmentResponse(
    Guid WarehouseId,
    string WarehouseName,
    decimal Qty,
    DateTime ShipmentDate);
