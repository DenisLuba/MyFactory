namespace MyFactory.WebApi.Contracts.Reports;

public record ReportsMonthlyProfitResponse(
    string Period,
    decimal Revenue,
    decimal ProductionCost,
    decimal Overhead,
    decimal Wages,
    decimal Profit
);

