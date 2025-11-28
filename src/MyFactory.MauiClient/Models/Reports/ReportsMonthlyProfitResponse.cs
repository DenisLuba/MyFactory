namespace MyFactory.MauiClient.Models.Reports;

public record ReportsMonthlyProfitResponse(
    string Period,
    decimal Revenue,
    decimal ProductionCost,
    decimal Overhead,
    decimal Wages,
    decimal Profit
);

