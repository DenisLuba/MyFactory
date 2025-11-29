namespace MyFactory.MauiClient.UIModels.Warehouse;

public record MaterialReceiptLineItem(
    string Material,
    decimal Quantity,
    string Unit,
    decimal Price,
    decimal TotalAmount
);