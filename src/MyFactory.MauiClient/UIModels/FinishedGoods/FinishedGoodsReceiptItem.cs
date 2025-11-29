namespace MyFactory.MauiClient.UIModels.FinishedGoods;

public record FinishedGoodsReceiptItem(
    string Product,
    int Quantity,
    string Date,
    string Warehouse,
    decimal TotalAmount
);