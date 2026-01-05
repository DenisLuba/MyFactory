namespace MyFactory.WebApi.Contracts.ProductionOrders;

public record ProductionOrderShipmentResponse(
    Guid WarehouseId,
    string WarehouseName,
    int Qty,
    DateTime ShipmentDate);
