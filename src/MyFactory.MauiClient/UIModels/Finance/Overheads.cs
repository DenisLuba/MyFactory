namespace MyFactory.MauiClient.UIModels.Finance;

// Накладные расходы
public record Overheads(
    string Date,
    string Article,
    decimal Amount,
    string Description,
    OverheadsStatus Status
);

