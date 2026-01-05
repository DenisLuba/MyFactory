namespace MyFactory.WebApi.Contracts.ProductionOrders;

public record CreateProductionOrderRequest(
    Guid SalesOrderItemId,
    Guid DepartmentId,
    int QtyPlanned);
