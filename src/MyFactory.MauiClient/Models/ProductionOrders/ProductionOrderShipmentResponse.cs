namespace MyFactory.MauiClient.Models.ProductionOrders;

public record ProductionOrderShipmentResponse(
    Guid WarehouseId,
    string WarehouseName,
    int Qty,
    DateTime ShipmentDate);
