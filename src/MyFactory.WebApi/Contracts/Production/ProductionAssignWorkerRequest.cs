namespace MyFactory.WebApi.Contracts.Production;

public record ProductionAssignWorkerRequest(
    Guid EmployeeId,
    int Quantity
);

