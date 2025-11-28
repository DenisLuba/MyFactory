namespace MyFactory.MauiClient.Models.Reports;

public record ReportsProductionCostResponse(
    Guid ProductionBatchId,
    decimal Cost
);

