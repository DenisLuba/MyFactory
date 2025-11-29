namespace MyFactory.MauiClient.UIModels.Finance;

public record PayrollItem(
    string Employee,
    string Period,
    decimal Accrued,
    decimal Paid,
    decimal Outstanding
);