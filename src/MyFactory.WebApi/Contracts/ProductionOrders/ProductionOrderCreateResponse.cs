namespace MyFactory.WebApi.Contracts.ProductionOrders;

public record ProductionOrderCreateResponse(
    Guid OrderId,
    string OrderNumber,
    string Status);
