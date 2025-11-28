namespace MyFactory.MauiClient.Models.Production;

public record ProductionAssignWorkerResponse(
    Guid OrderId,
    ProductionStatus Status);
