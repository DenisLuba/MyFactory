namespace MyFactory.WebApi.Contracts.Production;

public record ProductionCreateOrderResponse(
    Guid OrderId,
    ProductionStatus Status
);

