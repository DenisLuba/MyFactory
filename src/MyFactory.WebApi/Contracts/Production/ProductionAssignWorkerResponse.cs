namespace MyFactory.WebApi.Contracts.Production;

public record ProductionAssignWorkerResponse(
    Guid OrderId,
    ProductionStatus Status
);

