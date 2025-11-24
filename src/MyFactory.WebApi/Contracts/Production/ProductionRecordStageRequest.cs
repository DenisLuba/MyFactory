namespace MyFactory.WebApi.Contracts.Production;

public record ProductionRecordStageRequest(
    string Stage,       // "cutting", "sewing", "packaging"
    int Quantity
);

