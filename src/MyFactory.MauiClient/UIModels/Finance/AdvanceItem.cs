namespace MyFactory.MauiClient.UIModels.Finance;

public record AdvanceItem(
    string AdvanceNumber,
    string Employee,
    decimal AdvanceAmount,
    string Date,
    AdvanceStatus Status
);