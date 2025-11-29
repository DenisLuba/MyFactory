namespace MyFactory.MauiClient.UIModels.FinishedGoods;

public record ShipmentItem(
    string Customer,
    string Product,
    int Quantity,
    string Date,
    decimal TotalAmount
);