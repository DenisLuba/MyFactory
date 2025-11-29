namespace MyFactory.MauiClient.UIModels.FinishedGoods;

// Отгрузка (накладная)
public record ShipmentItem(
    string Customer,
    string ProductName,
    int Quantity,
    string Date,
    decimal TotalAmount
);