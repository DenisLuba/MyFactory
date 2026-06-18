namespace MyFactory.WebApi.Contracts.ProductionOrders;

public record ProductionOrderShipmentResponse(
    Guid WarehouseId,
    string WarehouseName,
    decimal Qty,
    DateTime ShipmentDate);
