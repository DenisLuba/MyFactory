namespace MyFactory.MauiClient.Models.Production;

public record ProductionGetOrderStatusResponse(
    Guid Id,
    ProductionStatus Status);
