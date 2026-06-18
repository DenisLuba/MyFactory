namespace MyFactory.MauiClient.Models.ProductionOrders;

public record UpdateProductionOrderRequest(
    Guid DepartmentId,
    decimal QtyPlanned);
