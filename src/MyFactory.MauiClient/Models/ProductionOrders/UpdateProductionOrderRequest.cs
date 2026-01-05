namespace MyFactory.MauiClient.Models.ProductionOrders;

public record UpdateProductionOrderRequest(
    Guid DepartmentId,
    int QtyPlanned);
