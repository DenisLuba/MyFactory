namespace MyFactory.MauiClient.Models.Production;

public record ProductionAssignWorkerRequest(
    Guid EmployeeId,
    int Quantity);
