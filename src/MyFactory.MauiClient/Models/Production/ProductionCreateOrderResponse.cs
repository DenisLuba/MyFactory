namespace MyFactory.MauiClient.Models.Production;

public record ProductionCreateOrderResponse(
    Guid OrderId,
    ProductionStatus Status);
