namespace MyFactory.MauiClient.Models.Production;

public record ProductionAllocateResponse(
    Guid OrderId,
    ProductionStatus Status);
