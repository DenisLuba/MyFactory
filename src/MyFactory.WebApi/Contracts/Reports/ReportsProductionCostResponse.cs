namespace MyFactory.WebApi.Contracts.Reports;

public record ReportsProductionCostResponse(
    Guid ProductionBatchId,
    decimal Cost
);

