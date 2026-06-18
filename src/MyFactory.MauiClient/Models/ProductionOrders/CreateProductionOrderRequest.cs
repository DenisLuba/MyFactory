namespace MyFactory.MauiClient.Models.ProductionOrders;

public record CreateProductionOrderRequest(
    Guid SalesOrderItemId,
    Guid DepartmentId,
    decimal QtyPlanned);
