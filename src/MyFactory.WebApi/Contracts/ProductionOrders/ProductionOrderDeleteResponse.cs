namespace MyFactory.WebApi.Contracts.ProductionOrders;

public record ProductionOrderDeleteResponse(
    Guid OrderId,
    bool IsDeleted);
