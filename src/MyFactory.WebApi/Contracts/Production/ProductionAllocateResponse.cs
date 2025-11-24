namespace MyFactory.WebApi.Contracts.Production;

public record ProductionAllocateResponse(
    Guid OrderId,
    ProductionStatus Status
);

