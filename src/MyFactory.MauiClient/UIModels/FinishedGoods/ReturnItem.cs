namespace MyFactory.MauiClient.UIModels.FinishedGoods;

// Возврат товара
public record ReturnItem(
    string Customer,
    string ProductName,
    int Quantity,
    string Date,
    string Reason,
    string Status
);