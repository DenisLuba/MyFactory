namespace MyFactory.WebApi.Contracts.Production;

public record ProductionRecordStageResponse(
    Guid OrderId,
    ProductionStatus Status
);

