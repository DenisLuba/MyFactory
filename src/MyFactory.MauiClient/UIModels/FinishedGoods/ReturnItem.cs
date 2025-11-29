namespace MyFactory.MauiClient.UIModels.FinishedGoods;

public record ReturnItem(
    string Customer,
    string Product,
    int Quantity,
    string Date,
    string Reason,
    string Status
);