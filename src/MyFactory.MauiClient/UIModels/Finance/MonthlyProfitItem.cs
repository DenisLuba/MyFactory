namespace MyFactory.MauiClient.UIModels.Finance;

public record MonthlyProfitItem(
    string Period,
    decimal Revenue,
    decimal ProductionCost,
    decimal Overhead,
    decimal Payroll,
    decimal Profit
);