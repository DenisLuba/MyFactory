namespace MyFactory.WebApi.Contracts.ProductionOrders;

public record ProductionOrderUpdateResponse(
    Guid OrderId,
    string Status);
