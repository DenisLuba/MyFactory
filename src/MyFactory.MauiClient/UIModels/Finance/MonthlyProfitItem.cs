namespace MyFactory.MauiClient.UIModels.Finance;

// ≈жемес€чна€ прибыль
public record MonthlyProfitItem(
    string Period,
    decimal Revenue,
    decimal ProductionCost,
    decimal Overhead,
    decimal Payroll,
    decimal Profit
);