namespace MyFactory.MauiClient.Models.Production;

public record ProductionRecordStageResponse(
    Guid OrderId,
    ProductionStatus Status);
