namespace MyFactory.WebApi.Contracts.ProductionOrders;

public record UpdateProductionOrderRequest(
    Guid DepartmentId,
    decimal QtyPlanned);
